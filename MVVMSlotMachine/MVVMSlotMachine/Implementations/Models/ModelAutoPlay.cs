﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using MVVMSlotMachine.Implementations.Controllers;
using MVVMSlotMachine.Implementations.Properties;
using MVVMSlotMachine.Interfaces.Controllers;
using MVVMSlotMachine.Interfaces.Logic;
using MVVMSlotMachine.Interfaces.Models;
using MVVMSlotMachine.Types;

namespace MVVMSlotMachine.Implementations.Models
{
    /// <summary>
    /// This class handles of all the functionality related to auto-play.
    /// The class contains the auto-play logic (methods), and maintains
    /// the state of the auto-play session through a number of properties
    /// </summary>
    public class ModelAutoPlay : PropertySource, IModelAutoPlay
    {
        #region Instance fields
        private Enums.AutoPlayState _currentAutoPlayState;
        private int _noOfRunsInAutoPlay;
        private int _updateThreshold;
        private int _percentCompleted;
        private double _percentPayback;
        private Dictionary<int, int> _autoRunData;
        private WheelSymbolList _symbols;

        private ICommandExtended _autoCommand;

        private ILogicCalculateWinnings _logicCalculateWinnings;
        private ILogicSymbolGenerator _logicSymbolGenerator;

        private BackgroundWorker _worker;
        private Mutex _mutex;
        private bool _didCancel; 
        #endregion

        #region Constructor
        public ModelAutoPlay(
            ILogicCalculateWinnings logicCalculateWinnings,
            ILogicSymbolGenerator logicSymbolGenerator,
            int noOfRunsInAutoPlay,
            int updateThreshold)
        {
            CurrentAutoPlayState = Enums.AutoPlayState.BeforeFirstInteraction;
            NoOfRuns = noOfRunsInAutoPlay;
            PercentCompleted = 0;
            PercentPayback = 100;
            _updateThreshold = updateThreshold;
            _autoRunData = new Dictionary<int, int>();
            _symbols = new WheelSymbolList();

            _autoCommand = new AutoPlayControllerCommand(this);

            _logicCalculateWinnings = logicCalculateWinnings;
            _logicSymbolGenerator = logicSymbolGenerator;

            _worker = null;
            _mutex = new Mutex();
            _didCancel = false;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the current state of the auto-play session.
        /// </summary>
        public Enums.AutoPlayState CurrentAutoPlayState
        {
            get { return _currentAutoPlayState; }
            set
            {
                _currentAutoPlayState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets/sets the number of runs to perform in the next 
        /// auto-play session. Changing this value effectively 
        /// resets the auto-play state.
        /// </summary>
        public int NoOfRuns
        {
            get { return _noOfRunsInAutoPlay; }
            set
            {
                _noOfRunsInAutoPlay = value;
                PercentCompleted = 0;
                CurrentAutoPlayState = Enums.AutoPlayState.BeforeFirstInteraction;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets/sets the progress in percent of the currently 
        /// running auto-play session.
        /// </summary>
        public int PercentCompleted
        {
            get { return _percentCompleted; }
            private set
            {
                _percentCompleted = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets/sets the percentage payback of the most 
        /// recent auto-play session.
        /// </summary>
        public double PercentPayback
        {
            get { return _percentPayback; }
            private set
            {
                _percentPayback = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Retrieve data from the most recent - or  
        /// currently executing - auto-play session
        /// The retrieval is done thread-safe
        /// </summary>
        public Dictionary<int, int> AutoRunData
        {
            get
            {
                _mutex.WaitOne();
                Dictionary<int, int> autoRunDataCopy = new Dictionary<int, int>();
                foreach (var item in _autoRunData)
                {
                    autoRunDataCopy.Add(item.Key, item.Value);
                }
                _mutex.ReleaseMutex();

                return autoRunDataCopy;
            }
        }

        /// <summary>
        /// Property to retrieve the command for initiating
        /// an auto-play session.
        /// </summary>
        public ICommandExtended AutoCommand
        {
            get { return _autoCommand; }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Invoke an auto-play session, with the specified number of runs.
        /// The auto-play session is invoked on a separate thread, using
        /// a BackgroundWorker objects
        /// </summary>
        public void Run(long noOfRuns)
        {
            CurrentAutoPlayState = Enums.AutoPlayState.Running;
            _logicSymbolGenerator.Reset();

            _didCancel = false;
            _percentCompleted = 0;
            _autoRunData = new Dictionary<int, int>();
            _worker = new BackgroundWorker();

            // Set up all the relevant properties of the background worker,
            // in particular the methods to call at various stages of the task.
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += AutoRun;
            _worker.ProgressChanged += AutoRunProgressUpdate;
            _worker.RunWorkerCompleted += AutoRunCompleted;

            // Start the BackgroundWorker
            _worker.RunWorkerAsync(noOfRuns);
        }

        /// <summary>
        /// Cancels the currently running auto-play session. More precisely,
        /// the method calls CancelAsync() on the background worker, which the
        /// background worker will then need to react on.
        /// </summary>
        public void Cancel()
        {
            _didCancel = true;
            _worker.CancelAsync();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Method called by the background worker's DoWork method
        /// </summary>
        private void AutoRun(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            long runsToComplete = (long)doWorkEventArgs.Argument;
            long runsBetweenUpdates = _updateThreshold / 100;
            long updatePoints = Math.Max(1, Math.Min(runsToComplete / runsBetweenUpdates, 100));

            // Main loop for auto-play: Keep playing for the specified number 
            // of runs, unless the auto-run session is cancelled.
            for (int runsCompleted = 0; runsCompleted < runsToComplete && !_worker.CancellationPending; runsCompleted++)
            {
                _symbols.Rotate(_logicSymbolGenerator);
                AddToAutoRunData(_symbols);
                ReportProgress(runsCompleted, runsToComplete, updatePoints);
            }

            // Main loop done, report final result
            doWorkEventArgs.Result = runsToComplete;
            ReportProgress(runsToComplete, runsToComplete, updatePoints);
        }

        /// <summary>
        /// Method called by the background worker's WorkerReportsProgress method
        /// </summary>
        private void AutoRunProgressUpdate(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            OnPropertyChanged(nameof(PercentCompleted));
        }

        /// <summary>
        /// Method called by the background worker's RunWorkerCompleted method
        /// </summary>
        private void AutoRunCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            // In case of cancellation; revert to initial state
            if (_didCancel)
            {
                PercentPayback = 0;
                CurrentAutoPlayState = Enums.AutoPlayState.BeforeFirstInteraction;
            }
            // Normal completion; calculate final payback percentage, and enter idle state
            else
            {
                int autoRunWinnings = _logicCalculateWinnings.CalculateTotalWinnings(_autoRunData);
                long runsCompleted = (long)runWorkerCompletedEventArgs.Result;
                PercentPayback = (autoRunWinnings * 100.0) / (runsCompleted * 1.0);
                CurrentAutoPlayState = Enums.AutoPlayState.Idle;
            }
        }

        /// <summary>
        /// This method will report progress if the condition for reporting
        /// progress is fulfilled. The reporting is conditional to avoid
        /// excessive updating of the UI during short runs. 
        /// </summary>
        private void ReportProgress(long runsCompleted, long runsToComplete, long updatePoints)
        {
            // If condition is fulfilled, call the ReportProgress method
            if (runsCompleted % (runsToComplete / updatePoints) == 0)
            {
                _percentCompleted = (int)((runsCompleted * 100) / runsToComplete);
                _worker.ReportProgress(_percentCompleted);
            }
        }

        /// <summary>
        /// Amend the auto-run data with the specified wheel symbols,
        /// which is the outcome of a single spin. This update is 
        /// done with exclusive access, to avoid threading issues.
        /// </summary>
        private void AddToAutoRunData(WheelSymbolList symbols)
        {
            _mutex.WaitOne();
            if (!_autoRunData.ContainsKey(symbols.NumericKey))
            {
                _autoRunData.Add(symbols.NumericKey, 0);
            }
            _autoRunData[symbols.NumericKey]++;
            _mutex.ReleaseMutex();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using MVVMSlotMachine.Controllers;
using MVVMSlotMachine.Implementations.Common;
using MVVMSlotMachine.Implementations.Properties;
using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Logic;
using MVVMSlotMachine.Interfaces.Models;

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
        private Types.Enums.AutoPlayState _currentAutoPlayState;
        private int _noOfRunsInAutoPlay;
        private int _percentCompleted;
        private double _percentPayback;
        private Dictionary<int, int> _autoRunData;
        private Dictionary<int, Types.Enums.WheelSymbol> _wheelSymbols;

        private ICommandExtended _autoCommand;

        private ILogicCalculateWinnings _logicCalculateWinnings;
        private ILogicSymbolGenerator _logicSymbolGenerator;

        private BackgroundWorker _worker;
        private Mutex _mutex;
        private bool _didCancel; 
        #endregion

        #region Constructors
        public ModelAutoPlay(
            ILogicCalculateWinnings logicCalculateWinnings,
            ILogicSymbolGenerator logicSymbolGenerator,
            int noOfRunsInAutoPlay)
        {
            CurrentAutoPlayState = Types.Enums.AutoPlayState.BeforeFirstInteraction;
            NoOfRuns = noOfRunsInAutoPlay;
            PercentCompleted = 0;
            PercentPayback = 100;
            _autoRunData = new Dictionary<int, int>();
            _wheelSymbols = new Dictionary<int, Types.Enums.WheelSymbol>();

            _autoCommand = new AutoPlayControllerCommand(this);

            _logicCalculateWinnings = logicCalculateWinnings;
            _logicSymbolGenerator = logicSymbolGenerator;

            _worker = null;
            _mutex = new Mutex();
            _didCancel = false;
        }

        public ModelAutoPlay()
            : this(Configuration.Implementations.LogicCalculateWinnings,
                   Configuration.Implementations.LogicSymbolGenerator,
                   Configuration.Implementations.Settings.NoOfRunsInAutoPlay)
        {
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the current state of the auto-play session.
        /// </summary>
        public Types.Enums.AutoPlayState CurrentAutoPlayState
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
                CurrentAutoPlayState = Types.Enums.AutoPlayState.BeforeFirstInteraction;
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
        /// Retrieve the data resulting from the
        /// most recent auto-play session
        /// </summary>
        public Dictionary<int, int> AutoRunData
        {
            get { return ConvertAutoRunDataNumericKey(); }
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
            CurrentAutoPlayState = Types.Enums.AutoPlayState.Running;

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
        /// the method call CancelAsync() on the background worker, which the
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

            // Main loop for auto-play: Keep playing for the specified number 
            // of runs, unless the auto-run session is cancelled.
            for (int runsCompleted = 0; runsCompleted < runsToComplete && !_worker.CancellationPending; runsCompleted++)
            {
                // Update auto-run data with exclusive access
                _mutex.WaitOne();
                AddToAutoRunData(SpinNoDelay().Values.ToList());
                _mutex.ReleaseMutex();

                // Report progress
                ReportProgress(runsCompleted, runsToComplete);
            }

            // Main loop done, report final result
            doWorkEventArgs.Result = runsToComplete;
            ReportProgress(runsToComplete, runsToComplete);
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
                CurrentAutoPlayState = Types.Enums.AutoPlayState.BeforeFirstInteraction;
            }
            // Normal completion; calculate final payback percentage, and enter idle state
            else
            {
                int autoRunWinnings = _logicCalculateWinnings.CalculateTotalWinnings(ConvertAutoRunDataSymbolsKey());
                long runsCompleted = (long)runWorkerCompletedEventArgs.Result;
                PercentPayback = (autoRunWinnings * 100.0) / (runsCompleted * 1.0);
                CurrentAutoPlayState = Types.Enums.AutoPlayState.Idle;
            }
        }

        /// <summary>
        /// This method will report progress if the condition for reporting
        /// progress is fulfilled. The reporting is conditional to avoid
        /// excessive updating of the UI during short runs. 
        /// </summary>
        private void ReportProgress(long runsCompleted, long runsToComplete)
        {
            long updateModifier = Configuration.Implementations.Settings.AutoPlayUpdateThreshold / 100;
            long updateInterval = Math.Max(1, Math.Min(runsToComplete / updateModifier, 100));

            // If condition is fulfilled, call the ReportProgress method
            if (runsCompleted % (runsToComplete / updateInterval) == 0)
            {
                _percentCompleted = (int)((runsCompleted * 100) / runsToComplete);
                _worker.ReportProgress(_percentCompleted);
            }
        }

        /// <summary>
        /// Perform a single spin of the wheels, without any delays
        /// </summary>
        private Dictionary<int, Types.Enums.WheelSymbol> SpinNoDelay()
        {
            for (int wheelNo = 0; wheelNo < Configuration.Constants.NoOfWheels; wheelNo++)
            {
                _wheelSymbols[wheelNo] = _logicSymbolGenerator.GetWheelSymbol();
            }

            return _wheelSymbols;
        }

        /// <summary>
        /// Amend the auto-run data with the specified wheel symbols,
        /// which is the outcome of a single spin. This update should be 
        /// done with exclusive access, to avoid threading issues.
        /// </summary>
        private void AddToAutoRunData(List<Types.Enums.WheelSymbol> wheelSymbols)
        {
            int key = WheelSymbolConverter.WheelSymbolsToKey(wheelSymbols);
            if (!_autoRunData.ContainsKey(key))
            {
                _autoRunData.Add(key, 0);
            }
            _autoRunData[key]++;
        }

        /// <summary>
        /// Converts the auto-run data to a format which can be processed
        /// by the winnings calculation logic. The conversion is done with
        /// exclusive data access, to avoid threading issues.
        /// </summary>
        private Dictionary<List<Types.Enums.WheelSymbol>, int> ConvertAutoRunDataSymbolsKey()
        {
            var convertedData = new Dictionary<List<Types.Enums.WheelSymbol>, int>();

            _mutex.WaitOne();
            foreach (var element in _autoRunData)
            {
                convertedData.Add(WheelSymbolConverter.KeyToWheelSymbols(element.Key), element.Value);
            }
            _mutex.ReleaseMutex();

            return convertedData;
        }

        /// <summary>
        /// Converts the auto-run data to a format which can be used
        /// by the view model. The conversion is done with exclusive
        /// data access, to avoid threading issues.
        /// </summary>
        private Dictionary<int, int> ConvertAutoRunDataNumericKey()
        {
            var convertedData = new Dictionary<int, int>();

            _mutex.WaitOne();
            foreach (var element in _autoRunData)
            {
                List<Types.Enums.WheelSymbol> symbols = WheelSymbolConverter.KeyToWheelSymbols(element.Key);

                Dictionary<Types.Enums.WheelSymbol, int> symbolCount = new Dictionary<Types.Enums.WheelSymbol, int>();
                foreach (Types.Enums.WheelSymbol symbol in Enum.GetValues(typeof(Types.Enums.WheelSymbol)))
                {
                    symbolCount.Add(symbol, symbols.FindAll(s => s == symbol).Count);
                }

                foreach (var item in symbolCount)
                {
                    if (item.Value > 1)
                    {
                        int key = WheelSymbolConverter.SymbolCountToKey(item.Key, item.Value);
                        if (!convertedData.ContainsKey(key))
                        {
                            convertedData.Add(key, 0);
                        }

                        convertedData[key] += element.Value;
                    }
                }
            }
            _mutex.ReleaseMutex();

            return convertedData;
        } 
        #endregion
    }
}

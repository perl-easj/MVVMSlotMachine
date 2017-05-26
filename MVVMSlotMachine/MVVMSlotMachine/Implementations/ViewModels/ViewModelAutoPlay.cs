using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using MVVMSlotMachine.Implementations.Common;
using MVVMSlotMachine.Implementations.Properties;
using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Logic;
using MVVMSlotMachine.Interfaces.Models;
using MVVMSlotMachine.Interfaces.Properties;
using MVVMSlotMachine.Interfaces.ViewModels;

namespace MVVMSlotMachine.Implementations.ViewModels
{
    /// <summary>
    /// View model for interacting with the auto-play model
    /// </summary>
    public class ViewModelAutoPlay : PropertySourceSink, IViewModelAutoPlay
    {
        #region Instance fields
        private IModelAutoPlay _modelAutoPlay;
        private ILogicAnalyticalCalculation _logicAnalyticalCalculation;
        private ITickScale _tickScale;
        private Dictionary<string, string> _nullAutoRunData; 
        #endregion

        #region Constructors
        public ViewModelAutoPlay(
            List<IPropertySource> propertySources, 
            IModelAutoPlay modelAutoPlay,
            ILogicAnalyticalCalculation logicAnalyticalCalculation,
            ITickScale tickScale)
            : base(propertySources)
        {
            _modelAutoPlay = modelAutoPlay;
            _logicAnalyticalCalculation = logicAnalyticalCalculation;
            _tickScale = tickScale;
            _nullAutoRunData = null;

            AddPropertyDependency(nameof(IModelAutoPlay.CurrentAutoPlayState), nameof(IViewModelAutoPlay.AutoCommand));
            AddPropertyDependency(nameof(IModelAutoPlay.CurrentAutoPlayState), nameof(IViewModelAutoPlay.AutoPlayStatusText));
            AddPropertyDependency(nameof(IModelAutoPlay.CurrentAutoPlayState), nameof(IViewModelAutoPlay.AutoPlayGoText));
            AddPropertyDependency(nameof(IModelAutoPlay.CurrentAutoPlayState), nameof(IViewModelAutoPlay.AutoPlayProgressBarVisibility));
            AddPropertyDependency(nameof(IModelAutoPlay.CurrentAutoPlayState), nameof(IViewModelAutoPlay.AutoRunDataText));
            AddPropertyDependency(nameof(IModelAutoPlay.PercentCompleted), nameof(IViewModelAutoPlay.AutoPlayStatusText));
            AddPropertyDependency(nameof(IModelAutoPlay.PercentCompleted), nameof(IViewModelAutoPlay.AutoPlayPercentCompleted));
            AddPropertyDependency(nameof(IModelAutoPlay.PercentCompleted), nameof(IViewModelAutoPlay.AutoRunDataText));
            AddPropertyDependency(nameof(IModelAutoPlay.NoOfRuns), nameof(IViewModelAutoPlay.NoOfRunsText));
            AddPropertyDependency(nameof(IModelAutoPlay.NoOfRuns), nameof(IViewModelAutoPlay.NoOfRunsTick));
            AddPropertyDependency(nameof(IModelAutoPlay.NoOfRuns), nameof(IViewModelAutoPlay.AutoRunDataText));
        }

        public ViewModelAutoPlay()
            : this(Configuration.Implementations.ViewModelAutoPlayPropertySources, 
                   Configuration.Implementations.ModelAutoPlay,
                   Configuration.Implementations.LogicAnalyticalCalculation,
                   Configuration.Implementations.Settings.TickScaleAutoPlay)
        {
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Text to display on the control to start/cancel
        /// an auto-play session
        /// </summary>
        public string AutoPlayGoText
        {
            get
            {
                if (_modelAutoPlay.CurrentAutoPlayState == Types.Enums.AutoPlayState.Running)
                {
                    return Configuration.Implementations.Messages.GenerateText(
                        Types.Enums.MessageType.Cancel, 
                        Types.Enums.MessagePostProcessing.InitialCaps);
                }

                return Configuration.Implementations.Messages.GenerateText(
                    Types.Enums.MessageType.Go, 
                    Types.Enums.MessagePostProcessing.InitialCaps);
            }
        }

        /// <summary>
        /// Text informing about the state of the
        /// current auto-play session
        /// </summary>
        public string AutoPlayStatusText
        {
            get
            {
                if (_modelAutoPlay.CurrentAutoPlayState == Types.Enums.AutoPlayState.Idle)
                {
                    string paybackText = Configuration.Implementations.Messages.GenerateText(Types.Enums.MessageType.PayBack);
                    string simulatedText = Configuration.Implementations.Messages.GenerateText(Types.Enums.MessageType.Simulated);
                    string runsText = Configuration.Implementations.Messages.GenerateText(Types.Enums.MessageType.Spins);

                    return string.Format("{0:0.00} % {2} ({3}, {1:0,0} {4})",
                        _modelAutoPlay.PercentPayback, _modelAutoPlay.NoOfRuns, paybackText, simulatedText, runsText);
                }
                else if (_modelAutoPlay.CurrentAutoPlayState == Types.Enums.AutoPlayState.Running)
                {
                    return _modelAutoPlay.PercentCompleted + " % " + 
                           Configuration.Implementations.Messages.GenerateText(Types.Enums.MessageType.Done);
                }
                else
                {
                    return Configuration.Implementations.Messages.GenerateText(
                        Types.Enums.MessageType.Ready, 
                        Types.Enums.MessagePostProcessing.InitialCaps);
                }
            }
        }

        /// <summary>
        /// Progress (measured in percent) of the current
        /// auto-play session
        /// </summary>
        public int AutoPlayPercentCompleted
        {
            get { return _modelAutoPlay.PercentCompleted; }
        }

        /// <summary>
        /// Retrieves the visibility setting of the progress bar.
        /// The progress bar is only visible during the actual auto-play.
        /// </summary>
        public Visibility AutoPlayProgressBarVisibility
        {
            get { return _modelAutoPlay.CurrentAutoPlayState == Types.Enums.AutoPlayState.Running ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Text informing about the number of runs to perform
        /// when an auto-play session is initiated
        /// </summary>
        public string NoOfRunsText
        {
            get { return string.Format("  {0:0,0} {1}", _modelAutoPlay.NoOfRuns, 
                         Configuration.Implementations.Messages.GenerateText(Types.Enums.MessageType.Spins)); }
        }

        /// <summary>
        /// Tracks the setting of a ticker-based control
        /// (e.g. a Slider) for setting the number of runs.
        /// </summary>
        public int NoOfRunsTick
        {
            get { return _tickScale.ScaleToTick(_modelAutoPlay.NoOfRuns); }
            set
            {
                _modelAutoPlay.NoOfRuns = _tickScale.TickToScale(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Retrives text data for the most recent auto-play session
        /// </summary>
        public Dictionary<string, string> AutoRunDataText
        {
            get
            {
                // If "null data" should be returned, we simply return that data
                if (ReturnNullAutoRunData)
                {
                    return NullAutoRunData;
                }

                // Iterate over all relevant combination of wheel symbols and wheel counts.
                // For each symbol/count combination:
                // 1) Get the number of entries in the auto-run data
                // 2) Calculate expected number of entries, according to probability sesttings
                // 3) Create text entry for the symbol/count combination
                Dictionary<string, string> autoRunData = new Dictionary<string, string>();
                for (int count = Configuration.Constants.NoOfWheels; count >= Configuration.Constants.NoOfWheels - 1; count--)
                {
                    foreach (Types.Enums.WheelSymbol symbol in Enum.GetValues(typeof(Types.Enums.WheelSymbol)))
                    {
                        int key = WheelSymbolConverter.SymbolCountToKey(symbol, count);
                        int runs = 0;

                        if (_modelAutoPlay.AutoRunData.ContainsKey(key))
                        {
                            runs = _modelAutoPlay.AutoRunData[key];
                        }

                        string keyStr = Enum.GetName(typeof(Types.Enums.WheelSymbol), symbol) + count;
                        double runsExpected = _logicAnalyticalCalculation.ProbabilityForSymbolCount(symbol, count) * _modelAutoPlay.NoOfRuns;

                        if (_modelAutoPlay.PercentCompleted > 0 && runsExpected > 0)
                        {
                            double percentOfExpected = (runs / (_modelAutoPlay.PercentCompleted * (runsExpected / 100.0))) * 100.0;
                            string valStr = string.Format("{0:0.00} % ", percentOfExpected);
                            autoRunData.Add(keyStr, valStr);
                        }
                        else
                        {
                            autoRunData.Add(keyStr, "---");
                        }
                    }
                }

                return autoRunData;
            }
        }

        /// <summary>
        /// Property for initiating an auto-play session.
        /// </summary>
        public ICommandExtended AutoCommand
        {
            get { return _modelAutoPlay.AutoCommand; }
        }
        #endregion

        #region Private properties
        /// <summary>
        /// Generates a "null version" of auto-run data. The data
        /// is only generated once.
        /// </summary>
        private Dictionary<string, string> NullAutoRunData
        {
            get
            {
                if (_nullAutoRunData == null)
                {
                    _nullAutoRunData = new Dictionary<string, string>();
                    int noOfWheels = Configuration.Constants.NoOfWheels;
                    for (int count = noOfWheels; count >= noOfWheels - 1; count--)
                    {
                        foreach (Types.Enums.WheelSymbol symbol in Enum.GetValues(typeof(Types.Enums.WheelSymbol)))
                        {
                            _nullAutoRunData.Add(Enum.GetName(typeof(Types.Enums.WheelSymbol), symbol) + count, "---");
                        }
                    }
                }

                return _nullAutoRunData;
            }
        }

        /// <summary>
        /// Condition defining whether or not we return "null data"
        /// instead of actual auto-run progress data.
        /// </summary>
        private bool ReturnNullAutoRunData
        {
            get
            {
                return (_modelAutoPlay.CurrentAutoPlayState == Types.Enums.AutoPlayState.Running && _modelAutoPlay.NoOfRuns < 500000
                     || _modelAutoPlay.CurrentAutoPlayState == Types.Enums.AutoPlayState.Idle && _modelAutoPlay.PercentCompleted < 100
                     || _modelAutoPlay.CurrentAutoPlayState == Types.Enums.AutoPlayState.BeforeFirstInteraction);
            }
        } 
        #endregion
    }
}

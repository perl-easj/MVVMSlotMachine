using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using MVVMSlotMachine.Implementations.Properties;
using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Properties;
using MVVMSlotMachine.Interfaces.ViewModels;

namespace MVVMSlotMachine.Implementations.ViewModels
{
    /// <summary>
    /// This class acts as a facade for all view models, such that views
    /// can use this class as their data context.
    /// </summary>
    public class ViewModelFacade : PropertySink,
        IViewModelNormalPlay, 
        IViewModelAutoPlay, 
        IViewModelProbabilitySetup, 
        IViewModelAnalyticalCalculation,
        IViewModelMachineSettings,
        IViewModelWinningsSetup
    {
        private IViewModelNormalPlay _viewModelNormalPlay;
        private IViewModelAutoPlay _viewModelAutoPlay;
        private IViewModelProbabilitySetup _viewModelProbabilitySetup;
        private IViewModelAnalyticalCalculation _viewModelAnalyticalCalculation;
        private IViewModelMachineSettings _viewModelMachineSettings;
        private IViewModelWinningsSetup _viewModelWinningsSetup;

        #region Constructors
        public ViewModelFacade(
            IViewModelNormalPlay viewModelNormalPlay,
            IViewModelAutoPlay viewModelAutoPlay,
            IViewModelProbabilitySetup viewModelProbabilitySetup,
            IViewModelAnalyticalCalculation viewModelAnalyticalCalculation,
            IViewModelMachineSettings viewModelMachineSettings,
            IViewModelWinningsSetup viewModelWinningsSetup,
            List<IPropertySource> propertySources)
            : base(propertySources)
        {
            _viewModelNormalPlay = viewModelNormalPlay;
            _viewModelAutoPlay = viewModelAutoPlay;
            _viewModelProbabilitySetup = viewModelProbabilitySetup;
            _viewModelAnalyticalCalculation = viewModelAnalyticalCalculation;
            _viewModelMachineSettings = viewModelMachineSettings;
            _viewModelWinningsSetup = viewModelWinningsSetup;

            AddPropertyDependency(nameof(CreditsText), nameof(IViewModelNormalPlay.CreditsText));
            AddPropertyDependency(nameof(NoOfCreditsText), nameof(IViewModelNormalPlay.NoOfCreditsText));
            AddPropertyDependency(nameof(PlayButtonText), nameof(IViewModelNormalPlay.PlayButtonText));
            AddPropertyDependency(nameof(StatusText), nameof(IViewModelNormalPlay.StatusText));
            AddPropertyDependency(nameof(WheelSource), nameof(IViewModelNormalPlay.WheelSource));

            AddPropertyDependency(nameof(AutoPlayGoText), nameof(IViewModelAutoPlay.AutoPlayGoText));
            AddPropertyDependency(nameof(AutoPlayStatusText), nameof(IViewModelAutoPlay.AutoPlayStatusText));
            AddPropertyDependency(nameof(AutoPlayPercentCompleted), nameof(IViewModelAutoPlay.AutoPlayPercentCompleted));
            AddPropertyDependency(nameof(AutoPlayPercentCompleted), nameof(IViewModelAutoPlay.AutoRunDataText));
            AddPropertyDependency(nameof(AutoPlayProgressBarVisibility), nameof(IViewModelAutoPlay.AutoPlayProgressBarVisibility));
            AddPropertyDependency(nameof(NoOfRunsText), nameof(IViewModelAutoPlay.NoOfRunsText));
            AddPropertyDependency(nameof(NoOfRunsTick), nameof(IViewModelAutoPlay.NoOfRunsTick));
            AddPropertyDependency(nameof(NoOfRunsTick), nameof(IViewModelAutoPlay.AutoRunDataText));
            AddPropertyDependency(nameof(AutoRunDataText), nameof(IViewModelAutoPlay.AutoRunDataText));

            AddPropertyDependency(nameof(ProbBell), nameof(IViewModelProbabilitySetup.ProbBell));
            AddPropertyDependency(nameof(ProbCherry), nameof(IViewModelProbabilitySetup.ProbCherry));
            AddPropertyDependency(nameof(ProbClover), nameof(IViewModelProbabilitySetup.ProbClover));
            AddPropertyDependency(nameof(ProbMelon), nameof(IViewModelProbabilitySetup.ProbMelon));
            AddPropertyDependency(nameof(ProbSeven), nameof(IViewModelProbabilitySetup.ProbSeven));
            AddPropertyDependency(nameof(ProbShoe), nameof(IViewModelProbabilitySetup.ProbShoe));

            AddPropertyDependency(nameof(WinningsList), nameof(IViewModelWinningsSetup.WinningsList));
            AddPropertyDependency(nameof(WinningsAmount), nameof(IViewModelWinningsSetup.WinningsAmount));
            AddPropertyDependency(nameof(WinningsSelected), nameof(IViewModelWinningsSetup.WinningsSelected));
            AddPropertyDependency(nameof(WinningsTick), nameof(IViewModelWinningsSetup.WinningsTick));

            AddPropertyDependency(nameof(TheoreticalWinningsPercentageText), nameof(IViewModelAnalyticalCalculation.TheoreticalWinningsPercentageText));
        }

        public ViewModelFacade()
            : this(Configuration.Implementations.ViewModelNormalPlay,
                   Configuration.Implementations.ViewModelAutoPlay,
                   Configuration.Implementations.ViewModelProbabilitySetup,
                   Configuration.Implementations.ViewModelAnalyticalCalculation,
                   Configuration.Implementations.ViewModelMachineSettings,
                   Configuration.Implementations.ViewModelWinningsSetup,
                   Configuration.Implementations.ViewModelProxyPropertySources)
        {
        } 
        #endregion

        #region Normal Play properties
        public Dictionary<int, string> WheelSource
        {
            get { return _viewModelNormalPlay.WheelSource; }
        }

        public string PlayButtonText
        {
            get { return _viewModelNormalPlay.PlayButtonText; }
        }

        public string CreditsText
        {
            get { return _viewModelNormalPlay.CreditsText; }
        }

        public string NoOfCreditsText
        {
            get { return _viewModelNormalPlay.NoOfCreditsText; }
        }

        public string StatusText
        {
            get { return _viewModelNormalPlay.StatusText; }
        }
        #endregion

        #region Auto Play properties
        public string AutoPlayGoText
        {
            get { return _viewModelAutoPlay.AutoPlayGoText; }
        }

        public string AutoPlayStatusText
        {
            get { return _viewModelAutoPlay.AutoPlayStatusText; }
        }

        public int AutoPlayPercentCompleted
        {
            get { return _viewModelAutoPlay.AutoPlayPercentCompleted; }
        }

        public Visibility AutoPlayProgressBarVisibility
        {
            get { return _viewModelAutoPlay.AutoPlayProgressBarVisibility; }
        }

        public string NoOfRunsText
        {
            get { return _viewModelAutoPlay.NoOfRunsText; }
        }

        public int NoOfRunsTick
        {
            get { return _viewModelAutoPlay.NoOfRunsTick; }
            set
            {
                _viewModelAutoPlay.NoOfRunsTick = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, string> AutoRunDataText
        {
            get { return _viewModelAutoPlay.AutoRunDataText; }
        }
        #endregion

        #region Probability properties
        public int ProbCherry
        {
            get { return _viewModelProbabilitySetup.ProbCherry; }
            set
            {
                _viewModelProbabilitySetup.ProbCherry = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TheoreticalWinningsPercentageText));
            }
        }

        public int ProbClover
        {
            get { return _viewModelProbabilitySetup.ProbClover; }
            set
            {
                _viewModelProbabilitySetup.ProbClover = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TheoreticalWinningsPercentageText));
            }
        }

        public int ProbBell
        {
            get { return _viewModelProbabilitySetup.ProbBell; }
            set
            {
                _viewModelProbabilitySetup.ProbBell = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TheoreticalWinningsPercentageText));
            }
        }

        public int ProbMelon
        {
            get { return _viewModelProbabilitySetup.ProbMelon; }
            set
            {
                _viewModelProbabilitySetup.ProbMelon = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TheoreticalWinningsPercentageText));
            }
        }

        public int ProbSeven
        {
            get { return _viewModelProbabilitySetup.ProbSeven; }
            set
            {
                _viewModelProbabilitySetup.ProbSeven = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TheoreticalWinningsPercentageText));
            }
        }

        public int ProbShoe
        {
            get { return _viewModelProbabilitySetup.ProbShoe; }
            set
            {
                _viewModelProbabilitySetup.ProbShoe = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TheoreticalWinningsPercentageText));
            }
        }
        #endregion

        #region Winnings properties
        public ObservableCollection<ItemViewModelWinningsEntry> WinningsList
        {
            get { return _viewModelWinningsSetup.WinningsList; }
        }

        public ItemViewModelWinningsEntry WinningsSelected
        {
            get { return _viewModelWinningsSetup.WinningsSelected; }
            set { _viewModelWinningsSetup.WinningsSelected = value; }
        }

        public int WinningsTick
        {
            get { return _viewModelWinningsSetup.WinningsTick; }
            set
            {
                _viewModelWinningsSetup.WinningsTick = value;
                OnPropertyChanged(nameof(TheoreticalWinningsPercentageText));
            }
        }

        public int WinningsAmount
        {
            get { return _viewModelWinningsSetup.WinningsAmount; }
        } 
        #endregion

        #region Calculation properties
        public string TheoreticalWinningsPercentageText
        {
            get { return _viewModelAnalyticalCalculation.TheoreticalWinningsPercentageText; }
        }
        #endregion

        #region Machine Settings properties
        public bool UILanguage
        {
            get { return _viewModelMachineSettings.UILanguage; }
            set { _viewModelMachineSettings.UILanguage = value; }
        }

        public bool UIImageSet
        {
            get { return _viewModelMachineSettings.UIImageSet; }
            set
            {
                _viewModelMachineSettings.UIImageSet = value;
                OnPropertyChanged(nameof(WheelSymbolImages));
                OnPropertyChanged(nameof(WinningsList));
            }
        }

        public Dictionary<string, string> WheelSymbolImages
        {
            get { return Configuration.Implementations.WheelImage.GetAllImageSources(); }
        }
        #endregion

        #region Command properties
        public ICommandExtended SpinCommand
        {
            get { return Configuration.Implementations.ViewModelNormalPlay.SpinCommand; }
        }

        public ICommandExtended AddCreditCommand
        {
            get { return Configuration.Implementations.ViewModelNormalPlay.AddCreditCommand; }
        }

        public ICommandExtended AutoCommand
        {
            get { return Configuration.Implementations.ViewModelAutoPlay.AutoCommand; }
        }
        #endregion
    }
}

using System.Collections.Generic;
using MVVMSlotMachine.Implementations.Common;
using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Logic;
using MVVMSlotMachine.Interfaces.Messages;
using MVVMSlotMachine.Interfaces.Models;
using MVVMSlotMachine.Interfaces.Properties;
using MVVMSlotMachine.Interfaces.ViewModels;
using MVVMSlotMachine.Interfaces.WheelImages;

namespace MVVMSlotMachine.Configuration
{
    /// <summary>
    /// Responsibilities:
    /// 1) Exposes interface-type properties to be used
    ///    throughout the application
    /// </summary>
    public static class Implementations
    {
        /// <summary>
        /// Ensures that dependency injection has been completed
        /// before accessing any of the properties.
        /// </summary>
        static Implementations()
        {
            _dependencyInjector = new DependencyInjector();
            _dependencyInjector.Setup();
        }

        #region Static instance fields
        private static ISettings _settings = new Settings(new DefaultSettings());
        private static DependencyInjector _dependencyInjector;

        private static IViewModelNormalPlay _viewModelNormalPlay;
        private static IViewModelAutoPlay _viewModelAutoPlay;
        private static IViewModelProbabilitySetup _viewModelProbabilitySetup;
        private static IViewModelAnalyticalCalculation _viewModelAnalyticalCalculation;
        private static IViewModelMachineSettings _viewModelMachineSettings;
        private static IViewModelWinningsSetup _viewModelWinningsSetup;

        private static List<IPropertySource> _viewModelProxyPropertySources;
        private static List<IPropertySource> _viewModelWinningsSetupPropertySources;
        private static List<IPropertySource> _viewModelProbabilitySetupPropertySources;
        private static List<IPropertySource> _viewModelAutoPlayPropertySources;
        private static List<IPropertySource> _viewModelNormalPlayPropertySources;

        private static IModelAutoPlay _modelAutoPlay;
        private static IModelNormalPlay _modelNormalPlay;

        private static ILogicAnalyticalCalculation _logicAnalyticalCalculation;
        private static ILogicWinningsSetup _logicWinningsSetup;
        private static ILogicCalculateWinnings _logicCalculateWinnings;
        private static ILogicProbabilitySetup _logicProbabilitySetup;
        private static ILogicSymbolGenerator _logicSymbolGenerator;

        private static IMessages _messages;
        private static IWheelImage _wheelImage;
        #endregion

        #region Public static properties
        public static ISettings Settings
        {
            get { return _settings; }
        }

        public static IViewModelNormalPlay ViewModelNormalPlay
        {
            get { return _viewModelNormalPlay; }
            set { _viewModelNormalPlay = value; }
        }

        public static IViewModelAutoPlay ViewModelAutoPlay
        {
            get { return _viewModelAutoPlay; }
            set { _viewModelAutoPlay = value; }
        }

        public static IViewModelProbabilitySetup ViewModelProbabilitySetup
        {
            get { return _viewModelProbabilitySetup; }
            set { _viewModelProbabilitySetup = value; }
        }

        public static IViewModelAnalyticalCalculation ViewModelAnalyticalCalculation
        {
            get { return _viewModelAnalyticalCalculation; }
            set { _viewModelAnalyticalCalculation = value; }
        }

        public static IViewModelMachineSettings ViewModelMachineSettings
        {
            get { return _viewModelMachineSettings; }
            set { _viewModelMachineSettings = value; }
        }

        public static IModelAutoPlay ModelAutoPlay
        {
            get { return _modelAutoPlay; }
            set { _modelAutoPlay = value; }
        }

        public static IModelNormalPlay ModelNormalPlay
        {
            get { return _modelNormalPlay; }
            set { _modelNormalPlay = value; }
        }

        public static ILogicAnalyticalCalculation LogicAnalyticalCalculation
        {
            get { return _logicAnalyticalCalculation; }
            set { _logicAnalyticalCalculation = value; }
        }

        public static ILogicWinningsSetup LogicWinningsSetup
        {
            get { return _logicWinningsSetup; }
            set { _logicWinningsSetup = value; }
        }

        public static ILogicCalculateWinnings LogicCalculateWinnings
        {
            get { return _logicCalculateWinnings; }
            set { _logicCalculateWinnings = value; }
        }

        public static ILogicProbabilitySetup LogicProbabilitySetup
        {
            get { return _logicProbabilitySetup; }
            set { _logicProbabilitySetup = value; }
        }

        public static ILogicSymbolGenerator LogicSymbolGenerator
        {
            get { return _logicSymbolGenerator; }
            set { _logicSymbolGenerator = value; }
        }

        public static IMessages Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        public static IWheelImage WheelImage
        {
            get { return _wheelImage; }
            set { _wheelImage = value; }
        }

        public static List<IPropertySource> ViewModelProxyPropertySources
        {
            get { return _viewModelProxyPropertySources; }
            set { _viewModelProxyPropertySources = value; }
        }

        public static List<IPropertySource> ViewModelWinningsSetupPropertySources
        {
            get { return _viewModelWinningsSetupPropertySources; }
            set { _viewModelWinningsSetupPropertySources = value; }
        }

        public static List<IPropertySource> ViewModelProbabilitySetupPropertySources
        {
            get { return _viewModelProbabilitySetupPropertySources; }
            set { _viewModelProbabilitySetupPropertySources = value; }
        }

        public static List<IPropertySource> ViewModelAutoPlayPropertySources
        {
            get { return _viewModelAutoPlayPropertySources; }
            set { _viewModelAutoPlayPropertySources = value; }
        }

        public static List<IPropertySource> ViewModelNormalPlayPropertySources
        {
            get { return _viewModelNormalPlayPropertySources; }
            set { _viewModelNormalPlayPropertySources = value; }
        }

        public static IViewModelWinningsSetup ViewModelWinningsSetup
        {
            get { return _viewModelWinningsSetup; }
            set { _viewModelWinningsSetup = value; }
        }

        public static int DefaultProbability(Types.Types.WheelSymbol symbol)
        {
            return _settings.InitialProbability(symbol);
        }

        public static int DefaultWinnings(Types.Types.WheelSymbol symbol, int noOfSymbols)
        {
            return _settings.InitialWinnings(symbol, noOfSymbols);
        }
        #endregion
    }
}
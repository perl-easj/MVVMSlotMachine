using System;
using System.Collections.Generic;
using MVVMSlotMachine.Implementations.Common;
using MVVMSlotMachine.Implementations.Logic;
using MVVMSlotMachine.Implementations.Messages;
using MVVMSlotMachine.Implementations.Models;
using MVVMSlotMachine.Implementations.ViewModels;
using MVVMSlotMachine.Implementations.WheelImages;
using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Messages;
using MVVMSlotMachine.Interfaces.Properties;
using MVVMSlotMachine.Interfaces.WheelImages;

namespace MVVMSlotMachine.Configuration
{
    /// <summary>
    /// Responsibilities:
    /// 1) Chooses specific implementations of interfaces
    /// 2) Establishes dependecies between implementations
    /// </summary>
    public class DependencyInjector
    {
        #region Setup method
        /// <summary>
        /// Setup chooses specific implementations of all interfaces,
        /// as well as specific values for numeric settings.
        /// </summary>
        public void Setup()
        {
            #region Default setup
            ISettingsReadOnly settings = new DefaultSettings();
            Implementations.Messages = MessagesFactory(settings.Language);
            Implementations.WheelImage = WheelImageFactory(settings.ImageSet);
            #endregion

            #region Logic/Model setup
            LogicWinningsSetup logicWinningsSetup = new LogicWinningsSetup();
            LogicProbabilitySetup logicProbabilitySetup = new LogicProbabilitySetup();
            Implementations.LogicWinningsSetup = logicWinningsSetup;
            Implementations.LogicProbabilitySetup = logicProbabilitySetup;

            LogicSymbolGenerator logicSymbolGenerator = new LogicSymbolGenerator();
            LogicCalculateWinnings logicCalculateWinnings = new LogicCalculateWinnings();
            Implementations.LogicSymbolGenerator = logicSymbolGenerator;
            Implementations.LogicCalculateWinnings = logicCalculateWinnings;

            LogicAnalyticalCalculation logicAnalyticalCalculation = new LogicAnalyticalCalculation();
            Implementations.LogicAnalyticalCalculation = logicAnalyticalCalculation;

            ModelAutoPlay modelAutoPlay = new ModelAutoPlay();
            ModelNormalPlay modelNormalPlay = new ModelNormalPlay();
            Implementations.ModelAutoPlay = modelAutoPlay;
            Implementations.ModelNormalPlay = modelNormalPlay;
            #endregion

            #region Property sources setup
            List<IPropertySource> viewModelWinningsSetupPropertySources = new List<IPropertySource> { logicWinningsSetup };
            List<IPropertySource> viewModelProbabilitySetupPropertySources = new List<IPropertySource> { logicProbabilitySetup };
            List<IPropertySource> viewModelNormalPlayPropertySources = new List<IPropertySource> { modelNormalPlay };
            List<IPropertySource> viewModelAutoPlayPropertySources = new List<IPropertySource> { modelAutoPlay };

            Implementations.ViewModelWinningsSetupPropertySources = viewModelWinningsSetupPropertySources;
            Implementations.ViewModelProbabilitySetupPropertySources = viewModelProbabilitySetupPropertySources;
            Implementations.ViewModelNormalPlayPropertySources = viewModelNormalPlayPropertySources;
            Implementations.ViewModelAutoPlayPropertySources = viewModelAutoPlayPropertySources;
            #endregion

            #region ViewModel sestup
            ViewModelMachineSettings viewModelMachineSettings = new ViewModelMachineSettings();
            ViewModelAnalyticalCalculation viewModelAnalyticalCalculation = new ViewModelAnalyticalCalculation();
            ViewModelProbabilitySetup viewModelProbabilitySetup = new ViewModelProbabilitySetup();
            ViewModelAutoPlay viewModelAutoPlay = new ViewModelAutoPlay();
            ViewModelNormalPlay viewModelNormalPlay = new ViewModelNormalPlay();
            ViewModelWinningsSetup viewModelWinningsSetup = new ViewModelWinningsSetup();

            Implementations.ViewModelMachineSettings = viewModelMachineSettings;
            Implementations.ViewModelProbabilitySetup = viewModelProbabilitySetup;
            Implementations.ViewModelAnalyticalCalculation = viewModelAnalyticalCalculation;
            Implementations.ViewModelAutoPlay = viewModelAutoPlay;
            Implementations.ViewModelNormalPlay = viewModelNormalPlay;
            Implementations.ViewModelWinningsSetup = viewModelWinningsSetup;
            #endregion

            #region ViewModel Proxy setup
            List<IPropertySource> viewModelProxyPropertySources = new List<IPropertySource>
            {
                viewModelMachineSettings,
                viewModelProbabilitySetup,
                viewModelAnalyticalCalculation,
                viewModelAutoPlay,
                viewModelNormalPlay,
                viewModelWinningsSetup
            };

            Implementations.ViewModelProxyPropertySources = viewModelProxyPropertySources; 
            #endregion
        }
        #endregion

        #region Factory methods
        /// <summary>
        /// Produces a specific implementation of 
        /// the IMessages interface
        /// </summary>
        public static IMessages MessagesFactory(Types.Enums.UILanguage language)
        {
            switch (language)
            {
                case Types.Enums.UILanguage.Danish:
                    return new MessagesDanish();
                case Types.Enums.UILanguage.English:
                    return new MessagesEnglish();
                default:
                    throw new ArgumentException(nameof(MessagesFactory));
            }
        }

        /// <summary>
        /// Produces a specific implementation of 
        /// the IWheelImage interface
        /// </summary>
        public static IWheelImage WheelImageFactory(Types.Enums.UIImageSet imageSet)
        {
            switch (imageSet)
            {
                case Types.Enums.UIImageSet.A:
                    return new WheelImageA();
                case Types.Enums.UIImageSet.B:
                    return new WheelImageB();
                default:
                    throw new ArgumentException(nameof(WheelImageFactory));
            }
        } 
        #endregion
    }
}
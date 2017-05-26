using MVVMSlotMachine.Implementations.Properties;
using MVVMSlotMachine.Interfaces.ViewModels;

namespace MVVMSlotMachine.Implementations.ViewModels
{
    /// <summary>
    /// View model for handling the general settings
    /// for the slot machine 
    /// </summary>
    public class ViewModelMachineSettings : PropertySource, IViewModelMachineSettings
    {
        /// <summary>
        /// Toggles the currently chosen UI language
        /// </summary>
        public bool UILanguage
        {
            get { return Configuration.Implementations.Settings.Language == Types.Enums.UILanguage.English; }
            set { Configuration.Implementations.Settings.Language = value ? Types.Enums.UILanguage.English : Types.Enums.UILanguage.Danish; }
        }

        /// <summary>
        /// Toggles the currently chosen UI image set
        /// </summary>
        public bool UIImageSet
        {
            get { return Configuration.Implementations.Settings.ImageSet == Types.Enums.UIImageSet.A; }
            set { Configuration.Implementations.Settings.ImageSet = value ? Types.Enums.UIImageSet.A : Types.Enums.UIImageSet.B; }
        }
    }
}
using MVVMSlotMachine.Configuration;
using MVVMSlotMachine.Implementations.Properties;
using MVVMSlotMachine.Interfaces.Logic;
using MVVMSlotMachine.Interfaces.ViewModels;

namespace MVVMSlotMachine.Implementations.ViewModels
{
    /// <summary>
    /// View model corresponding to the logic for 
    /// analytically calculating a winnings percentage
    /// </summary>
    class ViewModelAnalyticalCalculation : PropertySource, IViewModelAnalyticalCalculation
    {
        private ILogicAnalyticalCalculation _logicAnalyticalCalculation;

        #region Constructor
        public ViewModelAnalyticalCalculation(ILogicAnalyticalCalculation logicAnalyticalCalculation)
        {
            _logicAnalyticalCalculation = logicAnalyticalCalculation;
        }
        #endregion

        /// <summary>
        /// Text for the theoretical winnings percentage for the
        /// current probability/winnings setup.
        /// </summary>
        public string TheoreticalWinningsPercentageText
        {
            get
            {
                double percent = _logicAnalyticalCalculation.CalculatePaybackPercentage();
                string paybackText = Setup.RunTimeSettings.Messages.GenerateText(Types.Enums.MessageType.PayBack);
                string calculatedText = Setup.RunTimeSettings.Messages.GenerateText(Types.Enums.MessageType.Calculated);

                return string.Format("{0:0.00} % {1} ({2})", percent, paybackText, calculatedText);
            }
        }
    }
}

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

        #region Constructors
        public ViewModelAnalyticalCalculation(ILogicAnalyticalCalculation logicAnalyticalCalculation)
        {
            _logicAnalyticalCalculation = logicAnalyticalCalculation;
        }

        public ViewModelAnalyticalCalculation() 
            : this(Configuration.Implementations.LogicAnalyticalCalculation)
        {
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
                string paybackText = Configuration.Implementations.Messages.GenerateText(Types.Enums.MessageType.PayBack);
                string calculatedText = Configuration.Implementations.Messages.GenerateText(Types.Enums.MessageType.Calculated);

                return string.Format("{0:0.00} % {1} ({2})", percent, paybackText, calculatedText);
            }
        }
    }
}

using System.Collections.Generic;
using MVVMSlotMachine.Interfaces.Logic;
using MVVMSlotMachine.Types;

namespace MVVMSlotMachine.Implementations.Logic
{
    /// <summary>
    /// This class contain the logic for calculating winnings, 
    /// either for a single game outcome, or a set of game outcome data. 
    /// The current winnings settings are used.
    /// </summary>
    public class LogicCalculateWinnings : ILogicCalculateWinnings
    {
        private ILogicWinningsSetup _logicWinningsSetup;

        #region Constructors
        public LogicCalculateWinnings(ILogicWinningsSetup logicWinningsSetup)
        {
            _logicWinningsSetup = logicWinningsSetup;
        }

        public LogicCalculateWinnings()
            : this(Configuration.Implementations.LogicWinningsSetup)
        {
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Calculate winnings for a single game outcome.
        /// </summary>
        public int CalculateWinnings(WheelSymbolList symbols)
        {
            int winningsAmount = 0;
            foreach (var item in symbols.WheelSymbolCounts)
            {
                int newWinnings = _logicWinningsSetup.GetWinnings(item.Symbol, item.Count);
                winningsAmount = newWinnings > winningsAmount ? newWinnings : winningsAmount;
            }

            return winningsAmount;
        }

        /// <summary>
        /// Calculate total winnings for a set of game outcomes.
        /// </summary>
        public int CalculateTotalWinnings(Dictionary<int, int> runData)
        {
            int totalWinnings = 0;
            foreach (var element in runData)
            {
                totalWinnings += CalculateWinnings(new WheelSymbolList(element.Key)) * element.Value;
            }

            return totalWinnings;
        } 
        #endregion
    }
}
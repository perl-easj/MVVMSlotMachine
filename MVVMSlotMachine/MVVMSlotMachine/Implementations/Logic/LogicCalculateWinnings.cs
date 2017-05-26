using System;
using System.Collections.Generic;
using MVVMSlotMachine.Interfaces.Logic;

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
        public int CalculateWinnings(List<Types.Enums.WheelSymbol> wheelSymbols)
        {
            // Find the number of occurrences of each wheel symbol
            Dictionary<Types.Enums.WheelSymbol, int> symbolCount = new Dictionary<Types.Enums.WheelSymbol, int>();
            foreach (Types.Enums.WheelSymbol symbol in Enum.GetValues(typeof(Types.Enums.WheelSymbol)))
            {
                symbolCount.Add(symbol, wheelSymbols.FindAll(s => s == symbol).Count);
            }

            // Find the winnings associated with each symbol/count entry.
            // Only use the winnings with the highest amount.
            int winningsAmount = 0;
            foreach (var item in symbolCount)
            {
                int newWinnings = _logicWinningsSetup.GetWinnings(item.Key, item.Value);
                winningsAmount = newWinnings > winningsAmount ? newWinnings : winningsAmount;
            }

            return winningsAmount;
        }

        /// <summary>
        /// Calculate total winnings for a set of game outcomes.
        /// </summary>
        public int CalculateTotalWinnings(Dictionary<List<Types.Enums.WheelSymbol>, int> runData)
        {
            int totalWinnings = 0;
            foreach (var element in runData)
            {
                totalWinnings += CalculateWinnings(element.Key) * element.Value;
            }

            return totalWinnings;
        } 
        #endregion
    }
}
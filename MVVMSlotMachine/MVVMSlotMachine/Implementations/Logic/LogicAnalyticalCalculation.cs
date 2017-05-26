using System.Collections.Generic;
using MVVMSlotMachine.Implementations.Common;
using MVVMSlotMachine.Interfaces.Logic;

namespace MVVMSlotMachine.Implementations.Logic
{
    /// <summary>
    /// This class contains the logic for analytically calculating 
    /// wheel symbol probabilities and payback percentages
    /// </summary>
    public class LogicAnalyticalCalculation : ILogicAnalyticalCalculation
    {
        #region Instance fields
        private ILogicWinningsSetup _logicWinningsSetup;
        private ILogicCalculateWinnings _logicCalculateWinnings;
        private ILogicProbabilitySetup _logicProbabilitySetup; 
        #endregion

        #region Constructors
        public LogicAnalyticalCalculation(
            ILogicWinningsSetup logicWinningsSetup,
            ILogicCalculateWinnings logicCalculateWinnings,
            ILogicProbabilitySetup logicProbabilitySetup)
        {
            _logicWinningsSetup = logicWinningsSetup;
            _logicCalculateWinnings = logicCalculateWinnings;
            _logicProbabilitySetup = logicProbabilitySetup;
        }

        public LogicAnalyticalCalculation() 
            : this(Configuration.Implementations.LogicWinningsSetup, 
                   Configuration.Implementations.LogicCalculateWinnings, 
                   Configuration.Implementations.LogicProbabilitySetup) 
        {
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Calculate the payback percentage. The current 
        /// probability and winnings settings are used.
        /// </summary>
        public double CalculatePaybackPercentage()
        {
            double accumulatedWinnings = 0.0;

            // For each entry in the winnings settings:
            // 1) Find the winnings for the entry. If the winnings is non-zero, then
            //    2) Calculate the probability for the entry
            //    3) Amend the accumulated winnings with the contribution from this entry
            foreach (var item in _logicWinningsSetup.WinningsSettings)
            {
                List<Types.Enums.WheelSymbol> symbols = WheelSymbolConverter.KeyToWheelSymbols(item.Key);
                int winnings = _logicCalculateWinnings.CalculateWinnings(symbols);

                if (winnings > 0)
                {
                    // We assume all symbols in winnings entry are identical
                    accumulatedWinnings += (winnings * ProbabilityForSymbolCount(symbols[0], symbols.Count));
                }
            }

            return accumulatedWinnings * 100.0;
        }

        /// <summary>
        /// Calculate the probability for an outcome containing
        /// the specified number of the specified symbol.
        /// </summary>
        public double ProbabilityForSymbolCount(Types.Enums.WheelSymbol symbol, int count)
        {
            double probability = 1.0;
            int probabilityPercent = _logicProbabilitySetup.GetProbability(symbol);

            // Probability for symbol to appear "count" times
            for (int i = 0; i < count; i++)
            {
                probability = probability * (probabilityPercent * 1.0) / 100.0;
            }

            // Probability for symbol NOT to appear in rest of symbols
            for (int i = 0; i < Configuration.Constants.NoOfWheels - count; i++)
            {
                probability = probability * ((100 - probabilityPercent) * 1.0) / 100.0;
            }

            // Multiply by number of ways this outcome can appear
            probability = probability * Combinations(Configuration.Constants.NoOfWheels, count);

            return probability;
        } 
        #endregion

        #region Private methods
        private int Factorial(int n)
        {
            return n <= 1 ? 1 : n * Factorial(n - 1);
        }

        private long Combinations(int n, int p)
        {
            return Factorial(n) / (Factorial(n - p) * Factorial(p));
        } 
        #endregion
    }
}

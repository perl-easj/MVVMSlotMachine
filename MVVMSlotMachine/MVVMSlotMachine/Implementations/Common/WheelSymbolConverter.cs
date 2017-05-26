using System.Collections.Generic;

namespace MVVMSlotMachine.Implementations.Common
{
    /// <summary>
    /// Contains various methods for conversion between representations
    /// of wheel symbol collections
    /// List:          Direct representation of wheel symbol collection
    /// Symbol, count: Number of specified wheel symbols
    /// int:           Numeric representation where each digit corresponds to a wheel symbol
    /// </summary>
    public class WheelSymbolConverter
    {
        /// <summary>
        /// Convert from list of wheel symbols to integer. A list of N
        /// wheel symbols will produce an N-digit key
        /// </summary>
        public static int WheelSymbolsToKey(List<Types.Enums.WheelSymbol> wheelState)
        {
            int multiplier = 1;
            int result = 0;

            foreach (Types.Enums.WheelSymbol ws in wheelState)
            {
                result = result + multiplier * (int)ws;
                multiplier = multiplier * 10;
            }

            return result;
        }

        /// <summary>
        /// Convert from integer to list of wheel symbols. An N-digit key
        /// will produce a wheel symbol list of length N.
        /// </summary>
        public static List<Types.Enums.WheelSymbol> KeyToWheelSymbols(int key)
        {
            var wheelSymbols = new List<Types.Enums.WheelSymbol>();
            int residualKey = key;

            while (residualKey > 0)
            {
                int symbol = residualKey % 10;
                residualKey = residualKey / 10;
                wheelSymbols.Add((Types.Enums.WheelSymbol)symbol);
            }

            return wheelSymbols;
        }

        /// <summary>
        /// Convert from wheel symbol and count to integer. 
        /// If the count is N, an N-digit key is produced
        /// </summary>
        public static int SymbolCountToKey(Types.Enums.WheelSymbol wheelSymbol, int count)
        {
            // Create wheel symbol list corresponding to wheel symbol and count
            var wheelSymbols = new List<Types.Enums.WheelSymbol>();
            for (int i = 0; i < count; i++)
            {
                wheelSymbols.Add(wheelSymbol);
            }

            // Use existing conversion with generated symbol list
            return WheelSymbolsToKey(wheelSymbols);
        }
    }
}

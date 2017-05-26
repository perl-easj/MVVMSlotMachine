using System.Collections.Generic;

namespace MVVMSlotMachine.Interfaces.Logic
{
    /// <summary>
    /// Interface for managing the setup of winnings 
    /// for wheel symbol combinations.
    /// </summary>
    public interface ILogicWinningsSetup
    {
        /// <summary>
        /// Retrieve the current winnings settings, only including
        /// combinations paying a non-zero winning
        /// </summary>
        Dictionary<int, int> WinningsSettings { get; }

        /// <summary>
        /// Retrieve the current winnings settings, also including
        /// combinations paying zero winnings
        /// </summary>
        Dictionary<int, int> WinningsSettingsComplete { get; }

        /// <summary>
        /// Retrieve the winning amount for an outcome containing the
        /// specified number of the specified symbol.
        /// </summary>
        int GetWinnings(Types.Enums.WheelSymbol symbol, int noOfSymbols);

        /// <summary>
        /// Set the winning amount for an outcome containing the
        /// specified number of the specified symbol.
        /// </summary>
        void SetWinnings(Types.Enums.WheelSymbol symbol, int noOfSymbols, int winAmount);

        /// <summary>
        /// Set the winnings to the default winnings.
        /// </summary>
        void SetDefaultWinnings();
    }
}
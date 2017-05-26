using System.Collections.Generic;

namespace MVVMSlotMachine.Interfaces.Logic
{
    /// <summary>
    /// Interface for calculate winnings, either for a single game outcome,
    /// or a set of game outcome data. The current winnings settings are used.
    /// </summary>
    public interface ILogicCalculateWinnings
    {
        /// <summary>
        /// Calculate winnings for a single game outcome.
        /// </summary>
        int CalculateWinnings(List<Types.Enums.WheelSymbol> wheelSymbols);

        /// <summary>
        /// Calculate total winnings for a set of game outcomes.
        /// </summary>
        int CalculateTotalWinnings(Dictionary<List<Types.Enums.WheelSymbol>, int> runData);
    }
}
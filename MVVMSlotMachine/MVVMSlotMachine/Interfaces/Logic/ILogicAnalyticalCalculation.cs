namespace MVVMSlotMachine.Interfaces.Logic
{
    /// <summary>
    /// Interface for analytically calculating a winnings percentage
    /// </summary>
    public interface ILogicAnalyticalCalculation
    {
        /// <summary>
        /// Calculate the payback percentage, for the given 
        /// number of wheel symbol. The current probability and
        /// wheel settings are used.
        /// </summary>
        double CalculatePaybackPercentage(int noOfSymbolsInGame);

        /// <summary>
        /// Calculate the probability for an outcome containing
        /// the specified number of the specified symbol, given
        /// the specified total number of symbols in the game.
        /// </summary>
        double ProbabilityForSymbolCount(Types.Types.WheelSymbol symbol, int count, int noOfSymbolsInGame);
    }
}
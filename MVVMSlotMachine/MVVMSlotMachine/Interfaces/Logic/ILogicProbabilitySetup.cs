using System.Collections.Generic;

namespace MVVMSlotMachine.Interfaces.Logic
{
    /// <summary>
    /// Interface for managing the setup of 
    /// probabilities for wheel symbols.
    /// </summary>
    public interface ILogicProbabilitySetup
    {
        /// <summary>
        /// Retrieve the current probability settings
        /// </summary>
        Dictionary<Types.Types.WheelSymbol, int> ProbabilitySettings { get; }

        /// <summary>
        /// Retrieve the probability for generating the specified symbol.
        /// </summary>
        int GetProbability(Types.Types.WheelSymbol symbol);

        /// <summary>
        /// Set the probability for generating the specified symbol.
        /// </summary>
        void SetProbability(Types.Types.WheelSymbol symbol, int percentage);

        /// <summary>
        /// Set the probabilities to the default probabilities.
        /// </summary>
        void SetDefaultProbabilities();
    }
}
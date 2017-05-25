using System;
using MVVMSlotMachine.Interfaces.Logic;

namespace MVVMSlotMachine.Implementations.Logic
{
    /// <summary>
    /// This class contains logic for generating a wheel symbol, 
    /// based on current probability settings.
    /// </summary>
    public class LogicSymbolGenerator : ILogicSymbolGenerator
    {
        #region Instance fields
        private Random _randomGenerator;
        private Array _enumSymbols;

        private ILogicProbabilitySetup _logicProbabilitySetup;
        #endregion

        #region Constructors
        public LogicSymbolGenerator(ILogicProbabilitySetup logicProbabilitySetup)
        {          
            _randomGenerator = new Random();
            _enumSymbols = Enum.GetValues(typeof(Types.Types.WheelSymbol));

            _logicProbabilitySetup = logicProbabilitySetup;
        }

        public LogicSymbolGenerator()
            : this(Configuration.Implementations.LogicProbabilitySetup)
        {
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Generates a single wheel symbol, using the current probability settings.
        /// </summary>
        public Types.Types.WheelSymbol GetWheelSymbol()
        {
            int percent = _randomGenerator.Next(100);
            int accumulatedProbability = 0;

            foreach (Types.Types.WheelSymbol symbol in _enumSymbols)
            {
                accumulatedProbability += _logicProbabilitySetup.GetProbability(symbol);
                if (accumulatedProbability > percent)
                {
                    return symbol;
                }
            }

            // Code will only reach this point if there is 
            // an error in the probability definitions.
            throw new ArgumentException(nameof(GetWheelSymbol));
        } 
        #endregion
    }
}

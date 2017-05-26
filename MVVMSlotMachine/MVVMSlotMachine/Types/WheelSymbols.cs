using System;
using System.Collections.Generic;

namespace MVVMSlotMachine.Types
{
    public class WheelSymbols
    {
        private List<Types.Enums.WheelSymbol> _symbols;

        public WheelSymbols(List<Enums.WheelSymbol> symbols)
        {
            _symbols = symbols;
        }

        public WheelSymbols(Enums.WheelSymbol symbol, int count)
        {
            if (count < 1)
            {
                throw new ArgumentException(nameof(WheelSymbols));    
            }

            for (int i = 0; i < count; i++)
            {
                _symbols.Add(symbol);
            }
        }

        public List<Enums.WheelSymbol> Symbols
        {
            get { return _symbols; }
            private set { _symbols = value; }
        }

        public int Count
        {
            get { return _symbols.Count; }
        }

        public int NumberOf(Enums.WheelSymbol symbol)
        {
            return _symbols.FindAll((wheelSymbol => wheelSymbol == symbol)).Count;
        }

    }
}
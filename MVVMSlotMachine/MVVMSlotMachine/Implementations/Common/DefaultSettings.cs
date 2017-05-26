using MVVMSlotMachine.Configuration;
using MVVMSlotMachine.Interfaces.Common;

namespace MVVMSlotMachine.Implementations.Common
{
    /// <summary>
    /// Provides an implementation of the default settings
    /// </summary>
    public class DefaultSettings : ISettingsReadOnly
    {
        public int NoOfRotationsPerSpin
        {
            get { return 20; }
        }

        public int RotationDelayMilliSecs
        {
            get { return 100; }
        }

        public int InitialCredits
        {
            get { return 10; }
        }

        public int NoOfRunsInAutoPlay
        {
            get { return 1000; }
        }

        public int AutoPlayUpdateThreshold
        {
            get { return 1000000; }
        }

        public Types.Enums.UILanguage Language
        {
            get { return Types.Enums.UILanguage.English; }
        }

        public Types.Enums.UIImageSet ImageSet
        {
            get { return Types.Enums.UIImageSet.A; }
        }

        public int InitialProbability(Types.Enums.WheelSymbol symbol)
        {
            if (symbol == Types.Enums.WheelSymbol.Bell) return 10;
            if (symbol == Types.Enums.WheelSymbol.Cherry) return 30;
            if (symbol == Types.Enums.WheelSymbol.Clover) return 60;

            return 0;
        }

        public int InitialWinnings(Types.Enums.WheelSymbol symbol, int noOfSymbols)
        {
            int key = WheelSymbolConverter.SymbolCountToKey(symbol, noOfSymbols);

            if (key == WheelSymbolConverter.SymbolCountToKey(Types.Enums.WheelSymbol.Bell, 3)) return 100;
            if (key == WheelSymbolConverter.SymbolCountToKey(Types.Enums.WheelSymbol.Cherry, 3)) return 10;
            if (key == WheelSymbolConverter.SymbolCountToKey(Types.Enums.WheelSymbol.Clover, 3)) return 1;
            if (key == WheelSymbolConverter.SymbolCountToKey(Types.Enums.WheelSymbol.Bell, 2)) return 5;
            if (key == WheelSymbolConverter.SymbolCountToKey(Types.Enums.WheelSymbol.Cherry, 2)) return 1;

            return 0;
        }

        public ITickScale TickScaleWinnings
        {
            get { return new TickScale(Constants.TickScaleWinnings125Repeat); }
        }

        public ITickScale TickScaleAutoPlay
        {
            get { return new TickScale(Constants.TickScaleAutoPlay10Power); }
        }
    }
}
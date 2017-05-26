using System.Collections.Generic;

namespace MVVMSlotMachine.Implementations.ViewModels
{
    /// <summary>
    /// Helper class for displaying a winnings entry in a 
    /// collection-oriented UI control, e.g. a ListView.
    /// Ties together three wheel symbols and a winnings amount.
    /// </summary>
    public class ItemViewModelWinningsEntry
    {
        private List<Types.Enums.WheelSymbol> _symbols;
        private int _winAmount;

        public ItemViewModelWinningsEntry(List<Types.Enums.WheelSymbol> symbols, int winAmount)
        {
            _symbols = symbols;
            _winAmount = winAmount;
        }

        public List<Types.Enums.WheelSymbol> WheelSymbols
        {
            get { return _symbols; }
        }

        public string Image1Source
        {
            get { return _symbols.Count > 0 ? Configuration.Implementations.WheelImage.GetImageSource(_symbols[0]) : ""; }
        }

        public string Image2Source
        {
            get { return _symbols.Count > 1 ? Configuration.Implementations.WheelImage.GetImageSource(_symbols[1]) : ""; }
        }

        public string Image3Source
        {
            get { return _symbols.Count > 2 ? Configuration.Implementations.WheelImage.GetImageSource(_symbols[2]) : ""; }
        }

        public int WinningsAmount
        {
            get { return _winAmount; }
        }
    }
}
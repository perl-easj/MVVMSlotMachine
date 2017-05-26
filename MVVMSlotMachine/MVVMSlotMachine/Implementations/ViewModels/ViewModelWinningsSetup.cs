﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using MVVMSlotMachine.Implementations.Common;
using MVVMSlotMachine.Implementations.Properties;
using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Logic;
using MVVMSlotMachine.Interfaces.Properties;
using MVVMSlotMachine.Interfaces.ViewModels;

namespace MVVMSlotMachine.Implementations.ViewModels
{
    /// <summary>
    /// View model for the winnings part of the setup. 
    /// </summary>
    public class ViewModelWinningsSetup : PropertySourceSink, IViewModelWinningsSetup
    {
        #region Instance fields
        private ILogicWinningsSetup _logicWinningsSetup;
        private ITickScale _tickScale;
        private ItemViewModelWinningsEntry _itemViewModelWinningsEntry;
        private ObservableCollection<ItemViewModelWinningsEntry> _winList;
        #endregion

        #region Constructors
        public ViewModelWinningsSetup(
            List<IPropertySource> propertySources,
            ILogicWinningsSetup logicWinningsSetup,
            ITickScale tickScale)
            : base(propertySources)
        {
            _logicWinningsSetup = logicWinningsSetup;
            _tickScale = tickScale;
            _itemViewModelWinningsEntry = null;
            _winList = new ObservableCollection<ItemViewModelWinningsEntry>();
        }

        public ViewModelWinningsSetup() :
            this(Configuration.Implementations.ViewModelWinningsSetupPropertySources,
                 Configuration.Implementations.LogicWinningsSetup,
                 Configuration.Implementations.Settings.TickScaleWinnings)
        {
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Retrieves a collection of winnings entry objects,
        /// to display in a list-oriented control (e.g. a ListView).
        /// </summary>
        public ObservableCollection<ItemViewModelWinningsEntry> WinningsList
        {
            get
            {
                _winList.Clear();
                foreach (var winEntry in _logicWinningsSetup.WinningsSettingsComplete)
                {
                    _winList.Add(new ItemViewModelWinningsEntry(WheelSymbolConverter.KeyToWheelSymbols(winEntry.Key), winEntry.Value));
                }

                return _winList;
            }
        }

        /// <summary>
        /// Tracks the winnings entry currently selected by the user.
        /// </summary>
        public ItemViewModelWinningsEntry WinningsSelected
        {
            get { return _itemViewModelWinningsEntry; }
            set
            {
                // Selection is kept even if selection becomes null,
                // to enable continuous update of an entry
                if (value != null)
                {
                    _itemViewModelWinningsEntry = value;
                }
                OnPropertyChanged(nameof(WinningsTick));
                OnPropertyChanged(nameof(WinningsAmount));
            }
        }

        /// <summary>
        /// Tracks the setting of a ticker-based control
        /// (e.g. a Slider) for setting the winnings amount 
        /// for the currently selected winnings entry.
        /// Zero is returned if no entry is sselected.
        /// </summary>
        public int WinningsTick
        {
            get
            {
                if (_itemViewModelWinningsEntry == null)
                {
                    return 0;
                }

                return _tickScale.ScaleToTick(_logicWinningsSetup.GetWinnings(
                    _itemViewModelWinningsEntry.WheelSymbols[0],
                    _itemViewModelWinningsEntry.WheelSymbols.Count));
            }
            set
            {
                int newWinnings = _tickScale.TickToScale(value);
                if (_itemViewModelWinningsEntry != null)
                {
                    _logicWinningsSetup.SetWinnings(
                        _itemViewModelWinningsEntry.WheelSymbols[0],
                        _itemViewModelWinningsEntry.WheelSymbols.Count, newWinnings);
                    UpdateItem(_itemViewModelWinningsEntry.WheelSymbols, newWinnings);
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(WinningsAmount));
                }

            }
        }

        /// <summary>
        /// The winnings amount for the currently selected winnings entry.
        /// Zero is returned if no entry is sselected.
        /// </summary>
        public int WinningsAmount
        {
            get
            {
                if (_itemViewModelWinningsEntry == null)
                {
                    return 0;
                }

                return _logicWinningsSetup.GetWinnings(
                    _itemViewModelWinningsEntry.WheelSymbols[0],
                    _itemViewModelWinningsEntry.WheelSymbols.Count);
            }
        }

        /// <summary>
        /// Retrieves the set of currently active wheel symbol images.
        /// </summary>
        public Dictionary<string, string> WheelSymbolImages
        {
            get { return Configuration.Implementations.WheelImage.GetAllImageSources(); }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Update the winnings entry corresponding to the specified wheel symbols.
        /// </summary>
        private void UpdateItem(List<Types.Enums.WheelSymbol> symbols, int newWinnings)
        {
            for (int index = 0; index < _winList.Count; index++)
            {
                if (CompareSymbols(symbols, _winList[index].WheelSymbols))
                {
                    // We found the matching entry, so update it by deleting
                    // and re-inserting with the updated winnings amount.
                    _winList.RemoveAt(index);
                    _winList.Insert(index, new ItemViewModelWinningsEntry(symbols, newWinnings));
                    return;
                }
            }
        }

        /// <summary>
        /// Determines if two lists of wheel symbols are equal. 
        /// This is defined as the lists having the same length AND
        /// all symbols match at each position in the lists
        /// </summary>
        private bool CompareSymbols(List<Types.Enums.WheelSymbol> symbolsA, List<Types.Enums.WheelSymbol> symbolsB)
        {
            if (symbolsA.Count != symbolsB.Count)
            {
                return false;
            }

            for (int index = 0; index < symbolsA.Count; index++)
            {
                if (symbolsA[index] != symbolsB[index])
                {
                    return false;
                }
            }

            return true;
        } 
        #endregion
    }
}

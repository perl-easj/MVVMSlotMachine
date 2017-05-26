using System;
using System.Collections.Generic;
using MVVMSlotMachine.Configuration;
using MVVMSlotMachine.Implementations.Common;
using MVVMSlotMachine.Implementations.Properties;
using MVVMSlotMachine.Interfaces.Logic;

namespace MVVMSlotMachine.Implementations.Logic
{
    /// <summary>
    /// This class contains logic for managing the setup of 
    /// winnings for wheel symbol combinations.
    /// </summary>
    public class LogicWinningsSetup : PropertySource, ILogicWinningsSetup
    {
        private Dictionary<int, int> _winningsSettings;

        #region Constructor
        public LogicWinningsSetup()
        {
            _winningsSettings = new Dictionary<int, int>();
            SetDefaultWinnings();
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Retrieve the current winnings settings, only including
        /// combinations paying a non-zero winning
        /// </summary>
        public Dictionary<int, int> WinningsSettings
        {
            get { return _winningsSettings; }
        }

        /// <summary>
        /// Retrieve the current winnings settings, also including
        /// combinations paying zero winnings
        /// </summary>
        public Dictionary<int, int> WinningsSettingsComplete
        {
            get
            {
                Dictionary<int, int> completeWinnings = new Dictionary<int, int>();

                // Iterate over all wheel symbol, and all "count" values equal to
                // total number of wheels, and total number of wheels minus 1.
                for (int count = Configuration.Constants.NoOfWheels; count >= Configuration.Constants.NoOfWheels - 1; count--)
                {
                    foreach (Types.Enums.WheelSymbol symbol in Enum.GetValues(typeof(Types.Enums.WheelSymbol)))
                    {
                        int winnings = GetWinnings(symbol, count);
                        completeWinnings.Add(Common.WheelSymbolConverter.SymbolCountToKey(symbol, count), winnings);
                    }
                }

                return completeWinnings;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Retrieve the winning amount for an outcome containing the
        /// specified number of the specified symbol.
        /// </summary>
        public int GetWinnings(Types.Enums.WheelSymbol symbol, int count)
        {
            int entryKey = WheelSymbolConverter.SymbolCountToKey(symbol, count);
            if (!_winningsSettings.ContainsKey(entryKey))
            {
                return 0;
            }

            return _winningsSettings[entryKey];
        }

        /// <summary>
        /// Set the winning amount for an outcome containing the
        /// specified number of the specified symbol.
        /// </summary>
        public void SetWinnings(Types.Enums.WheelSymbol symbol, int count, int winAmount)
        {
            if (count < 1 || winAmount < 0)
            {
                throw new ArgumentException(nameof(SetWinnings));
            }

            int entryKey = WheelSymbolConverter.SymbolCountToKey(symbol, count);
            if (!_winningsSettings.ContainsKey(entryKey))
            {
                _winningsSettings.Add(entryKey, winAmount);
            }
            else
            {
                _winningsSettings[entryKey] = winAmount;
            }

            OnPropertyChanged(nameof(WinningsSettings)); ;
        }

        /// <summary>
        /// Set the winnings to the default winnings.
        /// </summary>
        public void SetDefaultWinnings()
        {
            for (int count = 1; count <= Constants.NoOfWheels; count++)
            {
                foreach (Types.Enums.WheelSymbol symbol in Enum.GetValues(typeof(Types.Enums.WheelSymbol)))
                {
                    int winnings = Configuration.Implementations.DefaultWinnings(symbol, count);
                    if (winnings > 0)
                    {
                        SetWinnings(symbol, count, winnings);
                    }
                }
            }
        } 
        #endregion
    }
}

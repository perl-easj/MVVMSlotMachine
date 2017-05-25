using MVVMSlotMachine.Configuration;
using MVVMSlotMachine.Interfaces.Common;

namespace MVVMSlotMachine.Implementations.Common
{
    /// <summary>
    /// This class will hold the currently valid settings. The settings
    /// are initialised with a corresponding read-only settings object.
    /// 
    /// Some settings are directly related to choice of implementation objects
    /// (Strategy pattern); if they change, the corresponding implementation
    /// setting is also updated.
    ///  </summary>
    public class Settings : ISettings
    {
        private ISettingsReadOnly _initialSettings;

        private int _noOfRotationsPerSpin;
        private int _rotationDelayMilliSecs;
        private int _initialCredits;
        private int _noOfRunsInAutoPlay;
        private int _autoPlayUpdateThreshold;
        private Types.Types.UILanguage _language;
        private Types.Types.UIImageSet _imageSet;
        private ITickScale _tickScaleWinnings;
        private ITickScale _tickScaleAutoPlay;

        /// <summary>
        /// Initialise the setting from a given read-only settings object.
        /// This will typically be a "default settings" object.
        /// </summary>
        /// <param name="initialSettings"></param>
        public Settings(ISettingsReadOnly initialSettings)
        {
            _initialSettings = initialSettings;

            _noOfRotationsPerSpin = initialSettings.NoOfRotationsPerSpin;
            _rotationDelayMilliSecs = initialSettings.RotationDelayMilliSecs;
            _initialCredits = initialSettings.InitialCredits;
            _noOfRunsInAutoPlay = initialSettings.NoOfRunsInAutoPlay;
            _autoPlayUpdateThreshold = initialSettings.AutoPlayUpdateThreshold;
            _language = initialSettings.Language;
            _imageSet = initialSettings.ImageSet;
            _tickScaleWinnings = initialSettings.TickScaleWinnings;
            _tickScaleAutoPlay = initialSettings.TickScaleAutoPlay;
        }

        #region Settings related to implementation objects
        public Types.Types.UILanguage Language
        {
            get { return _language; }
            set
            {
                _language = value;
                Configuration.Implementations.Messages = DependencyInjector.MessagesFactory(_language);
            }
        }

        public Types.Types.UIImageSet ImageSet
        {
            get { return _imageSet; }
            set
            {
                _imageSet = value;
                Configuration.Implementations.WheelImage = DependencyInjector.WheelImageFactory(_imageSet);
            }
        }
        #endregion

        #region Simple settings
        public int NoOfRotationsPerSpin
        {
            get { return _noOfRotationsPerSpin; }
            set { _noOfRotationsPerSpin = value; }
        }

        public int RotationDelayMilliSecs
        {
            get { return _rotationDelayMilliSecs; }
            set { _rotationDelayMilliSecs = value; }
        }

        public int InitialCredits
        {
            get { return _initialCredits; }
            set { _initialCredits = value; }
        }

        public int NoOfRunsInAutoPlay
        {
            get { return _noOfRunsInAutoPlay; }
            set { _noOfRunsInAutoPlay = value; }
        }

        public int AutoPlayUpdateThreshold
        {
            get { return _autoPlayUpdateThreshold; }
            set { _autoPlayUpdateThreshold = value; }
        }

        public int InitialProbability(Types.Types.WheelSymbol symbol)
        {
            return _initialSettings.InitialProbability(symbol);
        }

        public int InitialWinnings(Types.Types.WheelSymbol symbol, int noOfSymbols)
        {
            return _initialSettings.InitialWinnings(symbol, noOfSymbols);
        }

        public ITickScale TickScaleWinnings
        {
            get { return _tickScaleWinnings; }
            set { _tickScaleWinnings = value; }
        }

        public ITickScale TickScaleAutoPlay
        {
            get { return _tickScaleAutoPlay; }
            set { _tickScaleAutoPlay = value; }
        } 
        #endregion
    }
}

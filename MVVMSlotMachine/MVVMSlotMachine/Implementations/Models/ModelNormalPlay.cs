using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVVMSlotMachine.Controllers;
using MVVMSlotMachine.Implementations.Properties;
using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Logic;
using MVVMSlotMachine.Interfaces.Models;
using MVVMSlotMachine.Interfaces.Properties;

namespace MVVMSlotMachine.Implementations.Models
{
    /// <summary>
    /// This class handles of all the functionality related to normal play.
    /// The class contains the normal-play logic (methods), and maintains
    /// the state of the normal play session through a number of properties
    /// </summary>
    public class ModelNormalPlay : PropertySourceSink, IModelNormalPlay
    {
        #region Instance fields
        private int _noOfRotationsPerSpin;
        private int _rotationDelayMilliSecs;

        private int _noOfCredits;
        private Dictionary<int, Types.Enums.WheelSymbol> _wheelSymbols;
        private Types.Enums.NormalPlayState _currentNormalPlayState;        

        private ICommandExtended _spinCommand;
        private ICommandExtended _addCreditCommand;

        private ILogicCalculateWinnings _logicCalculateWinnings;
        private ILogicSymbolGenerator _logicSymbolGenerator;
        #endregion

        #region Constructors
        public ModelNormalPlay(
            ILogicCalculateWinnings logicCalculateWinnings,
            ILogicSymbolGenerator logicSymbolGenerator, 
            int initialCredits,
            int noOfRotationsPerSpin,
            int rotationDelayMilliSecs) : base(new List<IPropertySource>())
        {
            _noOfRotationsPerSpin = noOfRotationsPerSpin;
            _rotationDelayMilliSecs = rotationDelayMilliSecs;

            _noOfCredits = initialCredits;
            CurrentNormalPlayState = Types.Enums.NormalPlayState.BeforeFirstInteraction;
            _wheelSymbols = new Dictionary<int, Types.Enums.WheelSymbol>();
            for (int wheelNo = 0; wheelNo < Configuration.Constants.NoOfWheels; wheelNo++)
            {
                _wheelSymbols[wheelNo] = Types.Enums.WheelSymbol.Cherry;
            }

            _spinCommand = new SpinControllerCommand(this);
            _addCreditCommand = new AddCreditsControllerCommand(this);

            _logicCalculateWinnings = logicCalculateWinnings;
            _logicSymbolGenerator = logicSymbolGenerator;

            AddCommandDependency(nameof(IModelNormalPlay.CurrentNormalPlayState), _spinCommand);
            AddCommandDependency(nameof(IModelNormalPlay.CurrentNormalPlayState), _addCreditCommand);
        }

        public ModelNormalPlay()
            : this(Configuration.Implementations.LogicCalculateWinnings, 
                   Configuration.Implementations.LogicSymbolGenerator,
                   Configuration.Implementations.Settings.InitialCredits,
                   Configuration.Implementations.Settings.NoOfRotationsPerSpin,
                   Configuration.Implementations.Settings.RotationDelayMilliSecs)
        {
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets/sets the of credits available for the player.
        /// </summary>
        public int NoOfCredits
        {
            get { return _noOfCredits; }
            set
            {
                _noOfCredits = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the number of credits won after the most recent game.
        /// </summary>
        public int CreditsWon
        {
            get { return _logicCalculateWinnings.CalculateWinnings(_wheelSymbols.Values.ToList()); }
        }

        /// <summary>
        /// Gets/sets the wheels symbols currently showing
        /// </summary>
        public Dictionary<int, Types.Enums.WheelSymbol> WheelSymbols
        {
            get { return _wheelSymbols; }
            private set
            {
                _wheelSymbols = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets/sets the current state of the game session
        /// </summary>
        public Types.Enums.NormalPlayState CurrentNormalPlayState
        {
            get { return _currentNormalPlayState; }
            set
            {
                _currentNormalPlayState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property to retrieve the command for initiating
        /// a single game (i.e. a "spin").
        /// </summary>
        public ICommandExtended SpinCommand
        {
            get { return _spinCommand; }
        }

        /// <summary>
        /// Property to retrieve the command for adding
        /// a single credit to the player's balance
        /// </summary>
        public ICommandExtended AddCreditCommand
        {
            get { return _addCreditCommand; }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Invokes a single game (i.e. a "spin"). The game will take some  
        /// seconds to complete, and updates the WheelSymbols property after
        /// each rotation, causing the UI to update the wheel symbol images.
        /// </summary>
        public async Task Spin()
        {
            for (int rotation = 0; rotation < _noOfRotationsPerSpin; rotation++)
            {
                WheelSymbols = DoRotation();
                await Task.Delay(_rotationDelayMilliSecs);
            }

            OnPropertyChanged(nameof(CreditsWon));
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Performs a single "rotation" (i.e. update of wheel symbols)
        /// of all wheels.
        /// </summary>
        private Dictionary<int, Types.Enums.WheelSymbol> DoRotation()
        {
            for (int wheelNo = 0; wheelNo < Configuration.Constants.NoOfWheels; wheelNo++)
            {
                _wheelSymbols[wheelNo] = _logicSymbolGenerator.GetWheelSymbol();
            }
            return _wheelSymbols;
        }
        #endregion
    }
}

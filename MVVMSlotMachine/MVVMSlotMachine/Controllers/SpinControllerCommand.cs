using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Models;

namespace MVVMSlotMachine.Controllers
{
    /// <summary>
    /// Responsibilities:
    /// 1) Invokes logic for performing a single spin of a Slot Machine
    /// </summary>
    public class SpinControllerCommand : ICommandExtended
    {
        private IModelNormalPlay _modelNormalPlay;

        #region Constructors
        public SpinControllerCommand(IModelNormalPlay modelNormalPlay)
        {
            _modelNormalPlay = modelNormalPlay;
        }

        public SpinControllerCommand()
            : this(Configuration.Implementations.ModelNormalPlay)
        {
        }
        #endregion

        /// <summary>
        /// You can only start a game if the machine is idle, and 
        /// you have more than 0 (zero) credits.
        /// </summary>
        public override bool CanExecute(object parameter)
        {
            return _modelNormalPlay == null ||
                   (_modelNormalPlay.NoOfCredits > 0 &&
                    (_modelNormalPlay.CurrentNormalPlayState == Types.Types.NormalPlayState.Idle ||
                     _modelNormalPlay.CurrentNormalPlayState == Types.Types.NormalPlayState.BeforeFirstInteraction));
        }

        /// <summary>
        /// A game can run asyncronously, but we await the completion of the game,
        /// before querying for the outcome of the game.
        /// </summary>
        public override async void Execute(object parameter)
        {
            if (_modelNormalPlay != null)
            {
                _modelNormalPlay.NoOfCredits--;
                _modelNormalPlay.CurrentNormalPlayState = Types.Types.NormalPlayState.Spinning;

                await _modelNormalPlay.Spin();

                _modelNormalPlay.NoOfCredits += _modelNormalPlay.CreditsWon;
                _modelNormalPlay.CurrentNormalPlayState = Types.Types.NormalPlayState.Idle;
            }
        }
    }
}

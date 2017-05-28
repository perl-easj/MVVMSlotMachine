using MVVMSlotMachine.Interfaces.Controllers;
using MVVMSlotMachine.Interfaces.Models;

namespace MVVMSlotMachine.Implementations.Controllers
{
    /// <summary>
    /// Invokes logic for performing a single spin of a Slot Machine
    /// </summary>
    public class SpinControllerCommand : ICommandExtended
    {
        private IModelNormalPlay _modelNormalPlay;

        public SpinControllerCommand(IModelNormalPlay modelNormalPlay)
        {
            _modelNormalPlay = modelNormalPlay;
        }

        /// <summary>
        /// You can only start a game if the machine is idle, and 
        /// you have more than 0 (zero) credits.
        /// </summary>
        public override bool CanExecute(object parameter)
        {
            return _modelNormalPlay == null ||
                   (_modelNormalPlay.NoOfCredits > 0 &&
                    (_modelNormalPlay.CurrentNormalPlayState == Types.Enums.NormalPlayState.Idle ||
                     _modelNormalPlay.CurrentNormalPlayState == Types.Enums.NormalPlayState.BeforeFirstInteraction));
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
                _modelNormalPlay.CurrentNormalPlayState = Types.Enums.NormalPlayState.Spinning;

                await _modelNormalPlay.Spin();

                _modelNormalPlay.NoOfCredits += _modelNormalPlay.CreditsWon;
                _modelNormalPlay.CurrentNormalPlayState = Types.Enums.NormalPlayState.Idle;
            }
        }
    }
}

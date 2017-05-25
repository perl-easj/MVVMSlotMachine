using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Models;

namespace MVVMSlotMachine.Controllers
{
    /// <summary>
    /// Responsibilities:
    /// 1) Invokes logic for adding a single credit to a Slot Machine
    /// </summary>
    public class AddCreditsControllerCommand : ICommandExtended
    {
        private IModelNormalPlay _modelNormalPlay;

        #region Constructors
        public AddCreditsControllerCommand(IModelNormalPlay modelNormalPlay)
        {
            _modelNormalPlay = modelNormalPlay;
        }

        public AddCreditsControllerCommand()
            : this(Configuration.Implementations.ModelNormalPlay)
        {
        } 
        #endregion

        /// <summary>
        /// Credits can only be added when the machine is idle
        /// (or before the first play)
        /// </summary>
        public override bool CanExecute(object parameter)
        {
            return (_modelNormalPlay != null) && 
                   (_modelNormalPlay.CurrentNormalPlayState == Types.Types.NormalPlayState.Idle ||
                    _modelNormalPlay.CurrentNormalPlayState == Types.Types.NormalPlayState.BeforeFirstInteraction);
        }

        public override void Execute(object parameter)
        {
            _modelNormalPlay.NoOfCredits++;
        }
    }
}

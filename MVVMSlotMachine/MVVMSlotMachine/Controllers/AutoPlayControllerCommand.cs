using MVVMSlotMachine.Interfaces.Common;
using MVVMSlotMachine.Interfaces.Models;

namespace MVVMSlotMachine.Controllers
{
    /// <summary>
    /// Responsibilities:
    /// 1) Invokes logic for starting (or cancelling) Auto-play
    /// </summary>
    public class AutoPlayControllerCommand : ICommandExtended
    {
        private IModelAutoPlay _modelAutoPlay;

        #region Constructors
        public AutoPlayControllerCommand(IModelAutoPlay modelAutoPlay)
        {
            _modelAutoPlay = modelAutoPlay;
        }

        public AutoPlayControllerCommand()
            : this(Configuration.Implementations.ModelAutoPlay)
        {
        } 
        #endregion

        public override bool CanExecute(object parameter)
        {
            return _modelAutoPlay != null;
        }

        /// <summary>
        /// The command effectively toggles the state of the auto-play; if an auto-play
        /// session is already running, the command will cancel the session.
        /// </summary>
        public override void Execute(object parameter)
        {
            if (_modelAutoPlay.CurrentAutoPlayState == Types.Enums.AutoPlayState.Running)
            {
                _modelAutoPlay.Cancel();
            }
            else
            {
                _modelAutoPlay.Run(_modelAutoPlay.NoOfRuns);
            }
        }
    }
}

using System.Collections.Generic;
using MVVMSlotMachine.Interfaces.Common;

namespace MVVMSlotMachine.Interfaces.Models
{
    /// <summary>
    /// Interface for a model managing auto-play.
    /// </summary>
    public interface IModelAutoPlay
    {
        /// <summary>
        /// Current state of the auto-play session
        /// </summary>
        Types.Types.AutoPlayState CurrentAutoPlayState { get; set; }

        /// <summary>
        /// The number of games to play during an auto-play session
        /// </summary>
        int NoOfRuns { get; set; }

        /// <summary>
        /// Return the progress in percent of the currently 
        /// running auto-play session
        /// </summary>
        int PercentCompleted { get; }

        /// <summary>
        /// Percentage payback of the most recent auto-play session
        /// </summary>
        double PercentPayback { get; }

        /// <summary>
        /// Retrieve the data resulting from the
        /// most recent auto-play session
        /// </summary>
        Dictionary<int, int> AutoRunData { get; }

        /// <summary>
        /// Property to retrieve the command for initiating
        /// an auto-play session.
        /// </summary>
        ICommandExtended AutoCommand { get; }

        /// <summary>
        /// Invoke an auto-play session, with the specified number of runs
        /// </summary>
        void Run(long noOfRuns);

        /// <summary>
        /// Cancels the currently running auto-play session
        /// </summary>
        void Cancel();
    }
}
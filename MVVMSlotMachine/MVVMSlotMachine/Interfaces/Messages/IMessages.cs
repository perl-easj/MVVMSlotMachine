using System.Collections.Generic;

namespace MVVMSlotMachine.Interfaces.Messages
{
    /// <summary>
    /// Defines an interface for generating UI messages
    /// </summary>
    public interface IMessages
    {
        /// <summary>
        /// Generate the text for the specified message type, 
        /// using a single post-processing action
        /// </summary>
        string GenerateText(Types.Types.MessageType msgType, Types.Types.MessagePostProcessing postProcessAction);

        /// <summary>
        /// Generate the text for the specified message type, 
        /// using a list of post-processing actions
        /// </summary>
        string GenerateText(Types.Types.MessageType msgType, List<Types.Types.MessagePostProcessing> postProcessActions = null);
    }
}
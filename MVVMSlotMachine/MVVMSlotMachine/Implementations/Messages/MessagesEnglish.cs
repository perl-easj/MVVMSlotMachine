namespace MVVMSlotMachine.Implementations.Messages
{
    /// <summary>
    /// English translation of message types
    /// </summary>
    public class MessagesEnglish : MessagesBase
    {
        public MessagesEnglish()
        {
            SetTranslation(Types.Types.MessageType.Ready ,"ready");
            SetTranslation(Types.Types.MessageType.Play, "play");
            SetTranslation(Types.Types.MessageType.Go, "go");
            SetTranslation(Types.Types.MessageType.Spins, "runs");
            SetTranslation(Types.Types.MessageType.YouWon, "you won");
            SetTranslation(Types.Types.MessageType.Credit, "credit");
            SetTranslation(Types.Types.MessageType.Credits, "credits");
            SetTranslation(Types.Types.MessageType.Cancel, "cancel");
            SetTranslation(Types.Types.MessageType.Running, "running");
            SetTranslation(Types.Types.MessageType.SpinningWheels, "wheels are spinning");
            SetTranslation(Types.Types.MessageType.PayBack, "payback");
            SetTranslation(Types.Types.MessageType.Simulated, "simulated");
            SetTranslation(Types.Types.MessageType.Calculated, "calculated");
            SetTranslation(Types.Types.MessageType.Done, "done");
        }
    }
}
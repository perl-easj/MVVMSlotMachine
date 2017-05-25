namespace MVVMSlotMachine.Implementations.Messages
{
    /// <summary>
    /// Danish translation of message types
    /// </summary>
    public class MessagesDanish : MessagesBase
    {
        public MessagesDanish()
        {
            SetTranslation(Types.Types.MessageType.Ready, "klar");
            SetTranslation(Types.Types.MessageType.Play, "spil");
            SetTranslation(Types.Types.MessageType.Go, "kør");
            SetTranslation(Types.Types.MessageType.Spins, "kørsler");
            SetTranslation(Types.Types.MessageType.YouWon, "du vandt");
            SetTranslation(Types.Types.MessageType.Credit, "krone");
            SetTranslation(Types.Types.MessageType.Credits, "kroner");
            SetTranslation(Types.Types.MessageType.Cancel, "afbryd");
            SetTranslation(Types.Types.MessageType.Running, "kører");
            SetTranslation(Types.Types.MessageType.SpinningWheels, "hjulene drejer");
            SetTranslation(Types.Types.MessageType.PayBack, "tilbagebetaling");
            SetTranslation(Types.Types.MessageType.Simulated, "simuleret");
            SetTranslation(Types.Types.MessageType.Calculated, "beregnet");
            SetTranslation(Types.Types.MessageType.Done, "udført");
        }
    }
}
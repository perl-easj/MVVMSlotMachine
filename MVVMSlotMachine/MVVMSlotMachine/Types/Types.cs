namespace MVVMSlotMachine.Types
{
    /// <summary>
    /// Responsibilities:
    /// 1) Contains definitions of general types from the Slot Machine domain
    /// </summary>
    public class Types
    {
        #region Game types
        public enum NormalPlayState
        {
            BeforeFirstInteraction,
            Idle,
            Spinning
        }

        public enum AutoPlayState
        {
            BeforeFirstInteraction,
            Idle,
            Running
        }

        public enum WheelSymbol
        {
            Bell = 1,
            Cherry = 2,
            Clover = 3,
            Melon = 4,
            Seven = 5,
            Shoe = 6
        }
        #endregion

        #region Setup types
        public enum UILanguage
        {
            English,
            Danish
        }

        public enum UIImageSet
        {
            A,
            B
        }
        #endregion

        #region Message types
        public enum MessagePostProcessing
        {
            None,
            AddEllipsis,
            AddExclamationMark,
            AddQuestionMark,
            InitialCaps,
            AllCaps
        }

        public enum MessageType
        {
            Ready,
            Play,
            Go,
            Spins,
            YouWon,
            Credit,
            Credits,
            Cancel,
            Running,
            SpinningWheels,
            PayBack,
            Simulated,
            Calculated,
            Done
        } 
        #endregion
    }
}

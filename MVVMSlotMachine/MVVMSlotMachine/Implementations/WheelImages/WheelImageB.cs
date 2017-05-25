namespace MVVMSlotMachine.Implementations.WheelImages
{
    /// <summary>
    /// Specific set of wheel image sources
    /// </summary>
    public class WheelImageB : WheelImageBase
    {
        public WheelImageB()
        {
            SetImageSource(Types.Types.WheelSymbol.Bell, "..\\Assets\\ImageSetB\\Bell.jpg");
            SetImageSource(Types.Types.WheelSymbol.Cherry, "..\\Assets\\ImageSetB\\Cherry.jpg");
            SetImageSource(Types.Types.WheelSymbol.Clover, "..\\Assets\\ImageSetB\\Clover.jpg");
            SetImageSource(Types.Types.WheelSymbol.Melon, "..\\Assets\\ImageSetB\\Melon.jpg");
            SetImageSource(Types.Types.WheelSymbol.Seven, "..\\Assets\\ImageSetB\\Seven.jpg");
            SetImageSource(Types.Types.WheelSymbol.Shoe, "..\\Assets\\ImageSetB\\Shoe.jpg");
        }
    }
}

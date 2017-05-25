namespace MVVMSlotMachine.Implementations.WheelImages
{
    /// <summary>
    /// Specific set of wheel image sources
    /// </summary>
    public class WheelImageA : WheelImageBase
    {
        public WheelImageA()
        {
            SetImageSource(Types.Types.WheelSymbol.Bell, "..\\Assets\\ImageSetA\\Bell.jpg");
            SetImageSource(Types.Types.WheelSymbol.Cherry, "..\\Assets\\ImageSetA\\Cherry.jpg");
            SetImageSource(Types.Types.WheelSymbol.Clover, "..\\Assets\\ImageSetA\\Clover.jpg");
            SetImageSource(Types.Types.WheelSymbol.Melon, "..\\Assets\\ImageSetA\\Melon.jpg");
            SetImageSource(Types.Types.WheelSymbol.Seven, "..\\Assets\\ImageSetA\\Seven.jpg");
            SetImageSource(Types.Types.WheelSymbol.Shoe, "..\\Assets\\ImageSetA\\Shoe.jpg");
        }
    }
}

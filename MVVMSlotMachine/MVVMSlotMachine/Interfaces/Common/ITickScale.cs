namespace MVVMSlotMachine.Interfaces.Common
{
    /// <summary>
    /// Interface for tick-to-scale converters. A "tick" is a valid value
    /// on e.g. a Slider control (say, 0 to 9). These values may then be
    /// converted to other scale values (say, 10^0 to 10^9). The two
    /// methods should handle conversion in either direction.
    /// </summary>
    public interface ITickScale
    {
        /// <summary>
        /// Convert the given tick value to 
        /// corresponding scale value
        /// </summary>
        int TickToScale(int tickValue);

        /// <summary>
        /// Convert the given scale value to the corresponding tick value.
        /// Note that a value larger than the largest scale value is
        /// converted to the largest tick value, while a value smaller
        /// than the smallest scale value is converted to 0 (zero).
        /// </summary>
        /// <param name="scaleValue"></param>
        /// <returns></returns>
        int ScaleToTick(int scaleValue);
    }
}
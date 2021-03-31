namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// dwFlags
    /// Determines the function's return value if the point is not contained within any display monitor.
    /// </summary>
    internal enum MonitorFromFlag : uint
    {
        /// <summary>
        /// Returns NULL.
        /// </summary>
        DefaultToNull = 0,

        /// <summary>
        /// Returns a handle to the primary display monitor.
        /// </summary>
        DefaultToPrimary = 1,

        /// <summary>
        /// Returns a handle to the display monitor that is nearest to the point.
        /// </summary>
        DefaultToNearest = 2
    }
}

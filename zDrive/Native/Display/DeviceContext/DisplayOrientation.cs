namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// Contains possible values for the display orientation.
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-devmodea
    /// </summary>
    public enum DisplayOrientation : uint
    {
        /// <summary>
        /// DMDO_DEFAULT
        /// The display orientation is the natural orientation of the display device; it should be used as the default.
        /// No rotation.
        /// </summary>
        Identity = 0,

        /// <summary>
        /// DMDO_90
        /// The display orientation is rotated 90 degrees (measured clockwise) from DMDO_DEFAULT.
        /// 90 degree rotation.
        /// </summary>
        Rotate90Degree = 1,

        /// <summary>
        /// DMDO_180
        /// The display orientation is rotated 180 degrees (measured clockwise) from DMDO_DEFAULT.
        /// 180 degree rotation.
        /// </summary>
        Rotate180Degree = 2,

        /// <summary>
        /// DMDO_270
        /// The display orientation is rotated 270 degrees (measured clockwise) from DMDO_DEFAULT.
        /// 270 degree rotation.
        /// </summary>
        Rotate270Degree = 3
    }
}

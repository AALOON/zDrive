namespace zDrive.Native.Display.DisplayConfig
{
    /// <summary>
    /// Rotation modes
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ne-wingdi-displayconfig_rotation
    /// </summary>
    public enum DisplayConfigRotation : uint
    {
        /// <summary>
        /// Rotation mode is not specified
        /// </summary>
        NotSpecified = 0,

        /// <summary>
        /// Indicates that rotation is 0 degrees—landscape mode.
        /// </summary>
        Identity = 1,

        /// <summary>
        /// Indicates that rotation is 90 degrees clockwise—portrait mode.
        /// </summary>
        Rotate90 = 2,

        /// <summary>
        /// Indicates that rotation is 180 degrees clockwise—inverted landscape mode.
        /// </summary>
        Rotate180 = 3,

        /// <summary>
        /// Indicates that rotation is 270 degrees clockwise—inverted portrait mode.
        /// </summary>
        Rotate270 = 4
    }
}

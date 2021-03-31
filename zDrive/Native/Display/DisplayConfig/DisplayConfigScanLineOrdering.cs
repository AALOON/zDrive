namespace zDrive.Native.Display.DisplayConfig
{
    /// <summary>
    /// Possible values for display scan line ordering
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ne-wingdi-displayconfig_scanline_ordering
    /// </summary>
    public enum DisplayConfigScanLineOrdering : uint
    {
        /// <summary>
        /// Indicates that scan-line ordering of the output is unspecified.
        /// </summary>
        NotSpecified = 0,

        /// <summary>
        /// Indicates that the output is a progressive image.
        /// </summary>
        Progressive = 1,

        /// <summary>
        /// Indicates that the output is an interlaced image that is created beginning with the upper field.
        /// </summary>
        InterlacedWithUpperFieldFirst = 2,

        /// <summary>
        /// Indicates that the output is an interlaced image that is created beginning with the lower field.
        /// </summary>
        InterlacedWithLowerFieldFirst = 3
    }
}

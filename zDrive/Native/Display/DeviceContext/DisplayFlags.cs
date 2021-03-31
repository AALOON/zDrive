using System;

namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// Specifies the device's display mode. This member can be a combination of the following values.
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-devmodea
    /// </summary>
    [Flags]
    internal enum DisplayFlags : uint
    {
        None = 0,

        /// <summary>
        /// Specifies that the display is a noncolor device. If this flag is not set, color is assumed.
        /// </summary>
        GrayScale = 1,

        /// <summary>
        /// Specifies that the display mode is interlaced. If the flag is not set, noninterlaced is assumed.
        /// </summary>
        Interlaced = 2
    }
}

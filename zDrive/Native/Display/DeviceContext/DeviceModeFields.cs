using System;

namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// dmFields
    /// Specifies whether certain members of the DEVMODE structure have been initialized. If a member is initialized, its
    /// corresponding bit is set, otherwise the bit is clear. A driver supports only those DEVMODE members that are appropriate
    /// for the printer or display technology.
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-devmodea
    /// </summary>
    [Flags]
    internal enum DeviceModeFields : uint
    {
        None = 0,

        Position = 0x20,

        DisplayOrientation = 0x80,

        Color = 0x800,

        Duplex = 0x1000,

        YResolution = 0x2000,

        TtOption = 0x4000,

        Collate = 0x8000,

        FormName = 0x10000,

        LogPixels = 0x20000,

        BitsPerPixel = 0x40000,

        PelsWidth = 0x80000,

        PelsHeight = 0x100000,

        DisplayFlags = 0x200000,

        DisplayFrequency = 0x400000,

        DisplayFixedOutput = 0x20000000,

        AllDisplay = Position |
                     DisplayOrientation |
                     YResolution |
                     BitsPerPixel |
                     PelsWidth |
                     PelsHeight |
                     DisplayFlags |
                     DisplayFrequency |
                     DisplayFixedOutput
    }
}

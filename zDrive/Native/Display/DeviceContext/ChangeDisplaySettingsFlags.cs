using System;

namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// Flags for ChangeDisplaySettingsExA
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changedisplaysettingsexa
    /// </summary>
    [Flags]
    internal enum ChangeDisplaySettingsFlags : uint
    {
        None = 0x00000000,

        UpdateRegistry = 0x00000001,

        Test = 0x00000002,

        Fullscreen = 0x00000004,

        Global = 0x00000008,

        SetPrimary = 0x00000010,

        VideoParameters = 0x00000020,

        EnableUnsafeModes = 0x00000100,

        DisableUnsafeModes = 0x00000200,

        Reset = 0x40000000,

        ResetEx = 0x20000000,

        NoReset = 0x10000000
    }
}

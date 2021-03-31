using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DeviceContext.Structures
{
    /// <summary>
    /// The MONITORINFO structure contains information about a display monitor.
    /// The GetMonitorInfo function stores information in a MONITORINFO structure or a MONITORINFOEX structure.
    /// The MONITORINFO structure is a subset of the MONITORINFOEX structure. The MONITORINFOEX structure adds a string member
    /// to contain a name for the display monitor.
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-monitorinfo
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MonitorInfo
    {
        public uint Size;
        public readonly RectangleL Bounds;
        public readonly RectangleL WorkingArea;
        public readonly MonitorInfoFlags Flags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public readonly string DisplayName;

        public static MonitorInfo Initialize() =>
            new() { Size = (uint)Marshal.SizeOf(typeof(MonitorInfo)) };
    }
}

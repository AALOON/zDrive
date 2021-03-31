using System;

namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-monitorinfo
    /// </summary>
    [Flags]
    internal enum MonitorInfoFlags : uint
    {
        None = 0,

        /// <summary>
        /// MONITORINFOF_PRIMARY
        /// This is the primary display monitor.
        /// </summary>
        Primary = 1
    }
}

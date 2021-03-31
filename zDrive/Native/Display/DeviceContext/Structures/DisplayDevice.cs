using System.Runtime.InteropServices;

namespace zDrive.Native.Display.DeviceContext.Structures
{
    /// <summary>
    /// The DISPLAY_DEVICE structure receives information about the display device specified by the iDevNum parameter of the
    /// EnumDisplayDevices function.
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-display_devicea
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DisplayDevice
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint Size;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;

        [MarshalAs(UnmanagedType.U4)]
        public DisplayDeviceStateFlags StateFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;

        public static DisplayDevice Initialize() =>
            new() { Size = (uint)Marshal.SizeOf(typeof(DisplayDevice)) };
    }
}

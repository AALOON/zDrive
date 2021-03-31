using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_target_device_name
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DisplayConfigTargetDeviceName
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigDeviceInfoHeader Header;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigTargetDeviceNameFlags Flags;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigVideoOutputTechnology OutputTechnology;

        [MarshalAs(UnmanagedType.U2)]
        public readonly ushort EDIDManufactureId;

        [MarshalAs(UnmanagedType.U2)]
        public readonly ushort EDIDProductCodeId;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint ConnectorInstance;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public readonly string MonitorFriendlyDeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public readonly string MonitorDevicePath;


        public DisplayConfigTargetDeviceName(Luid adapter, uint targetId) : this() =>
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, targetId, this.GetType());
    }
}

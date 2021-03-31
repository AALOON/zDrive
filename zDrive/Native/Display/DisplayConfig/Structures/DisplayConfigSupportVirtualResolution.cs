using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_support_virtual_resolution
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DisplayConfigSupportVirtualResolution
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigDeviceInfoHeader Header;

        [MarshalAs(UnmanagedType.U4)]
        public readonly int DisableMonitorVirtualResolution;

        public bool IsDisableMonitorVirtualResolution => this.DisableMonitorVirtualResolution > 0;

        public DisplayConfigSupportVirtualResolution(Luid adapter, uint targetId) : this() =>
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, targetId, this.GetType(),
                DisplayConfigDeviceInfoType.GetSupportVirtualResolution);

        public DisplayConfigSupportVirtualResolution(Luid adapter, uint targetId, bool disableMonitorVirtualResolution)
            : this()
        {
            this.DisableMonitorVirtualResolution = disableMonitorVirtualResolution ? 1 : 0;
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, targetId, this.GetType(),
                DisplayConfigDeviceInfoType.SetSupportVirtualResolution);
        }
    }
}

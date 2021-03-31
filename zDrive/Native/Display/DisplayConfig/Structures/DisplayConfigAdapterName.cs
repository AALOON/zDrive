using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_adapter_name
    /// The DISPLAYCONFIG_ADAPTER_NAME structure contains information about the display adapter.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DisplayConfigAdapterName
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigDeviceInfoHeader Header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public readonly string AdapterDevicePath;

        public DisplayConfigAdapterName(Luid adapter) : this() =>
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, this.GetType());
    }
}

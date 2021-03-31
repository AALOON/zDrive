using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_target_base_type
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigTargetBaseType
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigDeviceInfoHeader Header;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigVideoOutputTechnology BaseOutputTechnology;

        public DisplayConfigTargetBaseType(Luid adapter, uint targetId) : this() =>
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, targetId, this.GetType());
    }
}

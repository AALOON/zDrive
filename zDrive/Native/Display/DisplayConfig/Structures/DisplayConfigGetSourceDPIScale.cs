using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigGetSourceDPIScale
    {
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigDeviceInfoHeader Header;

        [field: MarshalAs(UnmanagedType.U4)] public int MinimumScaleSteps { get; }

        [field: MarshalAs(UnmanagedType.U4)] public int CurrentScaleSteps { get; }

        [field: MarshalAs(UnmanagedType.U4)] public int MaximumScaleSteps { get; }

        public DisplayConfigGetSourceDPIScale(Luid adapter, uint sourceId) : this() =>
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, sourceId, this.GetType());
    }
}

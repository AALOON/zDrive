using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigSetSourceDPIScale
    {
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigDeviceInfoHeader Header;

        [field: MarshalAs(UnmanagedType.U4)] public int ScaleSteps { get; }

        public DisplayConfigSetSourceDPIScale(Luid adapter, uint sourceId, int scaleSteps) : this()
        {
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, sourceId, this.GetType());
            this.ScaleSteps = scaleSteps;
        }
    }
}

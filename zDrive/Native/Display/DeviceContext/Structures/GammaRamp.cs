using System.Runtime.InteropServices;

namespace zDrive.Native.Display.DeviceContext.Structures
{
    /// <summary>
    /// The GAMMARAMP structure is used by DrvIcmSetDeviceGammaRamp to set the hardware gamma ramp of a particular display
    /// device.
    /// https://docs.microsoft.com/en-us/windows/win32/api/winddi/ns-winddi-gammaramp
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct GammaRamp
    {
        public const int DataPoints = 256;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataPoints)]
        public readonly ushort[] Red;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataPoints)]
        public readonly ushort[] Green;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DataPoints)]
        public readonly ushort[] Blue;

        public GammaRamp(ushort[] red, ushort[] green, ushort[] blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
    }
}

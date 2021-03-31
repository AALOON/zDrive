using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DeviceContext.Structures
{
    /// <summary>
    /// The DEVMODE data structure contains information about the initialization and environment of a printer or a display
    /// device.
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-devmodea
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    internal struct DeviceMode
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        [FieldOffset(0)]
        public string DeviceName;

        [MarshalAs(UnmanagedType.U2)]
        [FieldOffset(32)]
        public ushort SpecificationVersion;

        [MarshalAs(UnmanagedType.U2)]
        [FieldOffset(34)]
        public ushort DriverVersion;

        [MarshalAs(UnmanagedType.U2)]
        [FieldOffset(36)]
        public ushort Size;

        [MarshalAs(UnmanagedType.U2)]
        [FieldOffset(38)]
        public ushort DriverExtra;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(40)]
        public DeviceModeFields Fields;

        [MarshalAs(UnmanagedType.Struct)]
        [FieldOffset(44)]
        public PointL Position;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(52)]
        public DisplayOrientation DisplayOrientation;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(56)]
        public DisplayFixedOutput DisplayFixedOutput;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(60)]
        public short Color;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(62)]
        public short Duplex;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(64)]
        public short YResolution;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(66)]
        public short TrueTypeOption;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(68)]
        public short Collate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        [FieldOffset(72)]
        public readonly string FormName;

        [MarshalAs(UnmanagedType.U2)]
        [FieldOffset(102)]
        public ushort LogicalInchPixels;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(104)]
        public uint BitsPerPixel;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(108)]
        public uint PixelsWidth;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(112)]
        public uint PixelsHeight;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(116)]
        public DisplayFlags DisplayFlags;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(120)]
        public uint DisplayFrequency;

        public DeviceMode(DeviceModeFields fields) : this()
        {
            this.SpecificationVersion = 0x0320;
            this.Size = (ushort)Marshal.SizeOf(this.GetType());
            this.Fields = fields;
        }

        public DeviceMode(string deviceName, DeviceModeFields fields) : this(fields) => this.DeviceName = deviceName;
    }
}

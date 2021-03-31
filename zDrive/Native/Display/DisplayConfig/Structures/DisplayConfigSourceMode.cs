using System;
using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_source_mode
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigSourceMode : IEquatable<DisplayConfigSourceMode>
    {
        public const ushort InvalidSourceModeIndex = 0xffff;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint Width;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint Height;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigPixelFormat PixelFormat;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly PointL Position;

        public DisplayConfigSourceMode(uint width, uint height, DisplayConfigPixelFormat pixelFormat,
            PointL position)
        {
            this.Width = width;
            this.Height = height;
            this.PixelFormat = pixelFormat;
            this.Position = position;
        }

        public bool Equals(DisplayConfigSourceMode other) =>
            this.Width == other.Width &&
            this.Height == other.Height &&
            this.PixelFormat == other.PixelFormat &&
            this.Position == other.Position;

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is DisplayConfigSourceMode mode && this.Equals(mode);
        }

        public override int GetHashCode() => HashCode.Combine(this.Width, this.Height, this.PixelFormat, this.Position);

        public static bool operator ==(DisplayConfigSourceMode left, DisplayConfigSourceMode right) =>
            Equals(left, right) || left.Equals(right);

        public static bool operator !=(DisplayConfigSourceMode left, DisplayConfigSourceMode right) => !(left == right);
    }
}

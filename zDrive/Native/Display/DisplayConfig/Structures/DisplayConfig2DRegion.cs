using System;
using System.Runtime.InteropServices;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_2dregion
    /// The DISPLAYCONFIG_2DREGION structure represents a point or an offset in a two-dimensional space.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfig2DRegion : IEquatable<DisplayConfig2DRegion>
    {
        [MarshalAs(UnmanagedType.U4)]
        public readonly uint Width;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint Height;

        public DisplayConfig2DRegion(uint width, uint height)
        {
            this.Width = width;
            this.Height = height;
        }

        public bool Equals(DisplayConfig2DRegion other) => this.Width == other.Width && this.Height == other.Height;

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is DisplayConfig2DRegion region && this.Equals(region);
        }

        public override int GetHashCode() => HashCode.Combine(this.Height, this.Width);

        public static bool operator ==(DisplayConfig2DRegion left, DisplayConfig2DRegion right) =>
            Equals(left, right) || left.Equals(right);

        public static bool operator !=(DisplayConfig2DRegion left, DisplayConfig2DRegion right) => !(left == right);
    }
}

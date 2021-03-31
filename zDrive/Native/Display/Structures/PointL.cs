using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Runtime.InteropServices;

namespace zDrive.Native.Display.Structures
{
    /// <summary>
    /// The POINTL structure defines the x- and y-coordinates of a point.
    /// https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-pointl
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PointL : IEquatable<PointL>
    {
        [MarshalAs(UnmanagedType.I4)]
        public int X;

        [MarshalAs(UnmanagedType.I4)]
        public int Y;

        [Pure]
        public Point ToPoint() => new(this.X, this.Y);

        [Pure]
        public Size ToSize() => new(this.X, this.Y);

        public PointL(Point point) : this(point.X, point.Y)
        {
        }

        public PointL(Size size) : this(size.Width, size.Height)
        {
        }

        public PointL(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(PointL other) => this.X == other.X && this.Y == other.Y;

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is PointL point && this.Equals(point);
        }

        public override int GetHashCode() => HashCode.Combine(this.X, this.Y);

        public static bool operator ==(PointL left, PointL right) => left.Equals(right);

        public static bool operator !=(PointL left, PointL right) => !(left == right);
    }
}

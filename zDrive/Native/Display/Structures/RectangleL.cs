using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Runtime.InteropServices;

namespace zDrive.Native.Display.Structures
{
    /// <summary>
    /// The RECTL structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
    /// https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rectl
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct RectangleL : IEquatable<RectangleL>
    {
        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(this.Left, this.Top, this.Right, this.Bottom);

        [MarshalAs(UnmanagedType.U4)]
        public readonly int Left;

        [MarshalAs(UnmanagedType.U4)]
        public readonly int Top;

        [MarshalAs(UnmanagedType.U4)]
        public readonly int Right;

        [MarshalAs(UnmanagedType.U4)]
        public readonly int Bottom;

        [Pure]
        public Rectangle ToRectangle() => new(this.Left, this.Top, this.Right - this.Left, this.Bottom - this.Top);

        public RectangleL(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public RectangleL(Rectangle rectangle) : this(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
        {
        }

        public bool Equals(RectangleL other) => this.Left == other.Left && this.Top == other.Top &&
                                                this.Right == other.Right && this.Bottom == other.Bottom;

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is RectangleL rectangle && this.Equals(rectangle);
        }

        public static bool operator ==(RectangleL left, RectangleL right) => left.Equals(right);

        public static bool operator !=(RectangleL left, RectangleL right) => !(left == right);
    }
}

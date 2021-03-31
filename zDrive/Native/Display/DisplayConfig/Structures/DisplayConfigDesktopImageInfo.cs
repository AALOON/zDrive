using System;
using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_desktop_image_info
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigDesktopImageInfo : IEquatable<DisplayConfigDesktopImageInfo>
    {
        public const ushort InvalidDesktopImageModeIndex = 0xffff;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly PointL PathSourceSize;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly RectangleL DesktopImageRegion;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly RectangleL DesktopImageClip;

        public DisplayConfigDesktopImageInfo(
            PointL pathSourceSize,
            RectangleL desktopImageRegion,
            RectangleL desktopImageClip)
        {
            this.PathSourceSize = pathSourceSize;
            this.DesktopImageRegion = desktopImageRegion;
            this.DesktopImageClip = desktopImageClip;
        }

        public bool Equals(DisplayConfigDesktopImageInfo other) =>
            this.PathSourceSize == other.PathSourceSize &&
            this.DesktopImageRegion == other.DesktopImageRegion &&
            this.DesktopImageClip == other.DesktopImageClip;

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is DisplayConfigDesktopImageInfo info && this.Equals(info);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.PathSourceSize.GetHashCode();
                hashCode = (hashCode * 397) ^ this.DesktopImageRegion.GetHashCode();
                hashCode = (hashCode * 397) ^ this.DesktopImageClip.GetHashCode();

                return hashCode;
            }
        }

        public static bool operator ==(DisplayConfigDesktopImageInfo left, DisplayConfigDesktopImageInfo right) =>
            Equals(left, right) || left.Equals(right);

        public static bool operator !=(DisplayConfigDesktopImageInfo left, DisplayConfigDesktopImageInfo right) =>
            !(left == right);
    }
}

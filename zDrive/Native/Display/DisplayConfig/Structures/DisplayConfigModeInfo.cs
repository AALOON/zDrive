using System;
using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_mode_info
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct DisplayConfigModeInfo : IEquatable<DisplayConfigModeInfo>
    {
        public const uint InvalidModeIndex = 0xffffffff;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(0)]
        public readonly DisplayConfigModeInfoType InfoType;

        [MarshalAs(UnmanagedType.U4)]
        [FieldOffset(4)]
        public readonly uint Id;

        [MarshalAs(UnmanagedType.Struct)]
        [FieldOffset(8)]
        public readonly Luid AdapterId;

        [MarshalAs(UnmanagedType.Struct)]
        [FieldOffset(16)]
        public readonly DisplayConfigTargetMode TargetMode;

        [MarshalAs(UnmanagedType.Struct)]
        [FieldOffset(16)]
        public readonly DisplayConfigSourceMode SourceMode;

        [MarshalAs(UnmanagedType.Struct)]
        [FieldOffset(16)]
        public readonly DisplayConfigDesktopImageInfo
            DesktopImageInfo;

        public DisplayConfigModeInfo(Luid adapterId, uint id, DisplayConfigTargetMode targetMode) : this()
        {
            this.AdapterId = adapterId;
            this.Id = id;
            this.TargetMode = targetMode;
            this.InfoType = DisplayConfigModeInfoType.Target;
        }

        public DisplayConfigModeInfo(Luid adapterId, uint id, DisplayConfigSourceMode sourceMode) : this()
        {
            this.AdapterId = adapterId;
            this.Id = id;
            this.SourceMode = sourceMode;
            this.InfoType = DisplayConfigModeInfoType.Source;
        }

        public DisplayConfigModeInfo(Luid adapterId, uint id, DisplayConfigDesktopImageInfo desktopImageInfo) : this()
        {
            this.AdapterId = adapterId;
            this.Id = id;
            this.DesktopImageInfo = desktopImageInfo;
            this.InfoType = DisplayConfigModeInfoType.DesktopImage;
        }

        public bool Equals(DisplayConfigModeInfo other) =>
            this.InfoType == other.InfoType &&
            this.Id == other.Id &&
            this.AdapterId == other.AdapterId &&
            ((this.InfoType == DisplayConfigModeInfoType.Source && this.SourceMode == other.SourceMode) ||
             (this.InfoType == DisplayConfigModeInfoType.Target && this.TargetMode == other.TargetMode) ||
             (this.InfoType == DisplayConfigModeInfoType.DesktopImage &&
              this.DesktopImageInfo == other.DesktopImageInfo));

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is DisplayConfigModeInfo info && this.Equals(info);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)this.InfoType;
                hashCode = (hashCode * 397) ^ (int)this.Id;
                hashCode = (hashCode * 397) ^ this.AdapterId.GetHashCode();

                switch (this.InfoType)
                {
                    case DisplayConfigModeInfoType.Source:
                        hashCode = (hashCode * 397) ^ this.SourceMode.GetHashCode();

                        break;
                    case DisplayConfigModeInfoType.Target:
                        hashCode = (hashCode * 397) ^ this.TargetMode.GetHashCode();

                        break;
                    case DisplayConfigModeInfoType.DesktopImage:
                        hashCode = (hashCode * 397) ^ this.DesktopImageInfo.GetHashCode();

                        break;
                    case DisplayConfigModeInfoType.Invalid:
                        hashCode = (hashCode * 397) ^ (int)InvalidModeIndex;
                        break;
                }

                return hashCode;
            }
        }

        public static bool operator ==(DisplayConfigModeInfo left, DisplayConfigModeInfo right) =>
            Equals(left, right) || left.Equals(right);

        public static bool operator !=(DisplayConfigModeInfo left, DisplayConfigModeInfo right) => !(left == right);
    }
}

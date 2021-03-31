using System;
using System.Runtime.InteropServices;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_video_signal_info
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigVideoSignalInfo : IEquatable<DisplayConfigVideoSignalInfo>
    {
        [MarshalAs(UnmanagedType.U8)]
        public readonly ulong PixelRate;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigRational HorizontalSyncFrequency;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigRational VerticalSyncFrequency;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfig2DRegion ActiveSize;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfig2DRegion TotalSize;

        [MarshalAs(UnmanagedType.U2)]
        public readonly VideoSignalStandard VideoStandard;

        [MarshalAs(UnmanagedType.U2)]
        public readonly ushort VerticalSyncFrequencyDivider;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigScanLineOrdering ScanLineOrdering;

        public DisplayConfigVideoSignalInfo(
            ulong pixelRate,
            DisplayConfigRational horizontalSyncFrequency,
            DisplayConfigRational verticalSyncFrequency,
            DisplayConfig2DRegion activeSize,
            DisplayConfig2DRegion totalSize,
            VideoSignalStandard videoStandard,
            ushort verticalSyncFrequencyDivider,
            DisplayConfigScanLineOrdering scanLineOrdering)
        {
            this.PixelRate = pixelRate;
            this.HorizontalSyncFrequency = horizontalSyncFrequency;
            this.VerticalSyncFrequency = verticalSyncFrequency;
            this.ActiveSize = activeSize;
            this.TotalSize = totalSize;
            this.VideoStandard = videoStandard;
            this.VerticalSyncFrequencyDivider = verticalSyncFrequencyDivider;
            this.ScanLineOrdering = scanLineOrdering;
        }

        public bool Equals(DisplayConfigVideoSignalInfo other) =>
            this.PixelRate == other.PixelRate &&
            this.HorizontalSyncFrequency == other.HorizontalSyncFrequency &&
            this.VerticalSyncFrequency == other.VerticalSyncFrequency &&
            this.ActiveSize == other.ActiveSize &&
            this.TotalSize == other.TotalSize &&
            this.VideoStandard == other.VideoStandard &&
            this.VerticalSyncFrequencyDivider == other.VerticalSyncFrequencyDivider &&
            this.ScanLineOrdering == other.ScanLineOrdering;

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is DisplayConfigVideoSignalInfo info && this.Equals(info);
        }

        public override int GetHashCode() => HashCode.Combine(this.PixelRate, this.HorizontalSyncFrequency,
            this.VerticalSyncFrequency, this.ActiveSize, this.TotalSize, this.VideoStandard,
            this.VerticalSyncFrequencyDivider, this.ScanLineOrdering);

        public static bool operator ==(DisplayConfigVideoSignalInfo left, DisplayConfigVideoSignalInfo right) =>
            Equals(left, right) || left.Equals(right);

        public static bool operator !=(DisplayConfigVideoSignalInfo left, DisplayConfigVideoSignalInfo right) =>
            !(left == right);
    }
}

using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_path_target_info
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigPathTargetInfo
    {
        [MarshalAs(UnmanagedType.Struct)]
        public readonly Luid AdapterId;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint TargetId;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint ModeInfoIndex;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigVideoOutputTechnology OutputTechnology;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigRotation Rotation;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigScaling Scaling;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigRational RefreshRate;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigScanLineOrdering ScanLineOrdering;

        [MarshalAs(UnmanagedType.Bool)]
        public readonly bool TargetAvailable;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigPathTargetInfoFlags StatusFlags;

        public ushort TargetModeInfoIndex => (ushort)((this.ModeInfoIndex << 16) >> 16);

        public ushort DesktopModeInfoIndex => (ushort)(this.ModeInfoIndex >> 16);

        public DisplayConfigPathTargetInfo(
            Luid adapterId,
            uint targetId,
            uint modeInfoIndex,
            DisplayConfigVideoOutputTechnology outputTechnology,
            DisplayConfigRotation rotation,
            DisplayConfigScaling scaling,
            DisplayConfigRational refreshRate,
            DisplayConfigScanLineOrdering scanLineOrdering,
            bool targetAvailable) : this()
        {
            this.AdapterId = adapterId;
            this.TargetId = targetId;
            this.ModeInfoIndex = modeInfoIndex;
            this.OutputTechnology = outputTechnology;
            this.Rotation = rotation;
            this.Scaling = scaling;
            this.RefreshRate = refreshRate;
            this.ScanLineOrdering = scanLineOrdering;
            this.TargetAvailable = targetAvailable;
        }

        public DisplayConfigPathTargetInfo(
            Luid adapterId,
            uint targetId,
            ushort targetModeInfoIndex,
            ushort desktopModeInfoIndex,
            DisplayConfigVideoOutputTechnology outputTechnology,
            DisplayConfigRotation rotation,
            DisplayConfigScaling scaling,
            DisplayConfigRational refreshRate,
            DisplayConfigScanLineOrdering scanLineOrdering,
            bool targetAvailable)
            : this(
                adapterId, targetId, 0, outputTechnology, rotation, scaling, refreshRate, scanLineOrdering,
                targetAvailable) =>
            this.ModeInfoIndex = (uint)(targetModeInfoIndex + (desktopModeInfoIndex << 16));
    }
}

using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_path_source_info
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigPathSourceInfo
    {
        public const ushort InvalidCloneGroupId = 0xffff;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly Luid AdapterId;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint SourceId;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint ModeInfoIndex;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigPathSourceInfoFlags StatusFlags;

        public ushort SourceModeInfoIndex => (ushort)((this.ModeInfoIndex << 16) >> 16);

        public ushort CloneGroupId => (ushort)(this.ModeInfoIndex >> 16);

        public DisplayConfigPathSourceInfo(Luid adapterId, uint sourceId, uint modeInfoIndex) : this()
        {
            this.AdapterId = adapterId;
            this.SourceId = sourceId;
            this.ModeInfoIndex = modeInfoIndex;
        }

        public DisplayConfigPathSourceInfo(
            Luid adapterId,
            uint sourceId,
            ushort sourceModeInfoIndex,
            ushort cloneGroupId) : this(adapterId, sourceId, 0) =>
            this.ModeInfoIndex = (uint)(sourceModeInfoIndex + (cloneGroupId << 16));
    }
}

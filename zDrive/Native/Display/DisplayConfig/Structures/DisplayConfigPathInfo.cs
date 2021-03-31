using System.Runtime.InteropServices;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_path_info
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigPathInfo
    {
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigPathSourceInfo SourceInfo;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigPathTargetInfo TargetInfo;

        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigPathInfoFlags Flags;

        public DisplayConfigPathInfo(
            DisplayConfigPathSourceInfo sourceInfo,
            DisplayConfigPathTargetInfo targetInfo,
            DisplayConfigPathInfoFlags flags)
        {
            this.SourceInfo = sourceInfo;
            this.TargetInfo = targetInfo;
            this.Flags = flags;
        }

        public DisplayConfigPathInfo(DisplayConfigPathSourceInfo sourceInfo, DisplayConfigPathInfoFlags flags)
        {
            this.SourceInfo = sourceInfo;
            this.Flags = flags;
            this.TargetInfo = new DisplayConfigPathTargetInfo();
        }
    }
}

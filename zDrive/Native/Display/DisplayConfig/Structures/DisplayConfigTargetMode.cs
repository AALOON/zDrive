using System;
using System.Runtime.InteropServices;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_target_mode
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigTargetMode : IEquatable<DisplayConfigTargetMode>
    {
        public const ushort InvalidTargetModeIndex = 0xffff;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigVideoSignalInfo TargetVideoSignalInfo;

        public DisplayConfigTargetMode(DisplayConfigVideoSignalInfo targetVideoSignalInfo) =>
            this.TargetVideoSignalInfo = targetVideoSignalInfo;

        public bool Equals(DisplayConfigTargetMode other) => this.TargetVideoSignalInfo == other.TargetVideoSignalInfo;

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is DisplayConfigTargetMode mode && this.Equals(mode);
        }

        public override int GetHashCode() => this.TargetVideoSignalInfo.GetHashCode();

        public static bool operator ==(DisplayConfigTargetMode left, DisplayConfigTargetMode right) =>
            Equals(left, right) || left.Equals(right);

        public static bool operator !=(DisplayConfigTargetMode left, DisplayConfigTargetMode right) => !(left == right);
    }
}

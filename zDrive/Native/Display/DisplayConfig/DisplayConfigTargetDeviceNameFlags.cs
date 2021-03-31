using System;

namespace zDrive.Native.Display.DisplayConfig
{
    [Flags]
    internal enum DisplayConfigTargetDeviceNameFlags : uint
    {
        None = 0,
        FriendlyNameFromEdid = 1,
        FriendlyNameForced = 2,
        EdidIdsValid = 4
    }
}

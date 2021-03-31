using System;

namespace zDrive.Native.Display.DisplayConfig
{
    [Flags]
    internal enum DisplayConfigPathTargetInfoFlags : uint
    {
        None = 0,
        InUse = 1,
        Forcible = 2,
        AvailabilityBoot = 3,
        AvailabilityPath = 4,
        AvailabilitySystem = 5
    }
}

using System;

namespace zDrive.Native.Display.DisplayConfig
{
    [Flags]
    internal enum DisplayConfigPathSourceInfoFlags : uint
    {
        None = 0,
        InUse = 1
    }
}

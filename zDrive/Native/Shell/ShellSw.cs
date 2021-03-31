using System;

namespace zDrive.Native.Shell
{
    [Flags]
    public enum ShellSw
    {
        Hide = 0,
        ShowNormal = 1,
        ShowMinimized = 2,
        MaximizeOrShowMaximized = 3,
        ShowNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActive = 7,
        ShowNa = 8,
        Restore = 9,
        ShowDefault = 10
    }
}

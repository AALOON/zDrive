using System;

namespace zDrive.Native.Shell
{
    /// <summary>
    /// A combination of one or more of the following values that indicate the content and validity of the other structure
    /// members:
    /// </summary>
    [Flags]
    public enum ShellSeeMask : uint
    {
        Default = 0x00000000,
        ClassName = 0x00000001,
        ClassKey = 0x00000003,
        IdList = 0x00000004,
        InvokeIdList = 0x0000000C,
        Icon = 0x00000010,
        HotKey = 0x00000020,
        NoCloseProcess = 0x00000040,
        ConnectNetDrv = 0x00000080,
        FlagDdeWaitOrNoAsync = 0x00000100,
        DoEnvSubst = 0x00000200,
        FlagNoUi = 0x00000400,
        Unicode = 0x00004000,
        NoConsole = 0x00008000,
        AsyncOk = 0x00100000,
        NoQueryClassStore = 0x01000000,
        HMonitor = 0x00200000,
        NoZoneChecks = 0x00800000,
        WaitForInputIdle = 0x02000000,
        FlagLogUsage = 0x04000000,
        FlagHInstIsSite = 0x08000000
    }
}

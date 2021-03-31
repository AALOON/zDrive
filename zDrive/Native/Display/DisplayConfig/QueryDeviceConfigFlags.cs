using System;

namespace zDrive.Native.Display.DisplayConfig
{
    /// <summary>
    /// The type of information to retrieve for QueryDisplayConfig.
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-querydisplayconfig
    /// </summary>
    [Flags]
    public enum QueryDeviceConfigFlags : uint
    {
        /// <summary>
        /// QDC_ALL_PATHS
        /// All the possible path combinations of sources to targets.
        /// </summary>
        /// <remarks>
        /// In the case of any temporary modes, the QDC_ALL_PATHS setting
        /// means the mode data returned may not be the same as that which is stored in the persistence database.
        /// </remarks>
        AllPaths = 0x00000001,

        /// <summary>
        /// QDC_ONLY_ACTIVE_PATHS
        /// Currently active paths only.
        /// </summary>
        /// <remarks>
        /// In the case of any temporary modes, the QDC_ONLY_ACTIVE_PATHS setting
        /// means the mode data returned may not be the same as that which is stored in the persistence database.
        /// </remarks>
        OnlyActivePaths = 0x00000002,

        /// <summary>
        /// QDC_DATABASE_CURRENT
        /// Active path as defined in the CCD database for the currently connected displays.
        /// </summary>
        DatabaseCurrent = 0x00000004,

        /// <summary>
        /// QDC_VIRTUAL_MODE_AWARE
        /// </summary>
        VirtualModeAware = 0x0000010
    }
}

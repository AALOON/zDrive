using System;

namespace zDrive.Native.Display.DisplayConfig
{
    /// <summary>
    /// The DISPLAYCONFIG_TOPOLOGY_ID enumeration specifies the type of display topology.
    /// Possible topology identifications.
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ne-wingdi-displayconfig_topology_id
    /// </summary>
    [Flags]
    public enum DisplayConfigTopologyId : uint
    {
        /// <summary>
        /// Invalid topology identification
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the display topology is an internal configuration.
        /// </summary>
        Internal = 0x00000001,

        /// <summary>
        /// Indicates that the display topology is clone-view configuration.
        /// </summary>
        Clone = 0x00000002,

        /// <summary>
        /// Indicates that the display topology is an extended configuration.
        /// </summary>
        Extend = 0x00000004,

        /// <summary>
        /// Indicates that the display topology is an external configuration.
        /// </summary>
        External = 0x00000008
    }
}

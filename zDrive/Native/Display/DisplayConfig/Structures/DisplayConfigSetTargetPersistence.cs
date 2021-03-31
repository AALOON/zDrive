using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_set_target_persistence
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigSetTargetPersistence
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigDeviceInfoHeader Header;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint BootPersistenceOn;

        public bool BootPersistence => this.BootPersistenceOn > 0;

        public DisplayConfigSetTargetPersistence(Luid adapter, uint targetId, bool bootPersistence) : this()
        {
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, targetId, this.GetType());
            this.BootPersistenceOn = bootPersistence ? 1u : 0u;
        }
    }
}

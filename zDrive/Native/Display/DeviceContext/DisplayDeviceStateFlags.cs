using System;

namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// Device state flags. It can be any reasonable combination of the following.
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-display_devicea
    /// https://stackoverflow.com/questions/58500275/valid-friendly-monitor-names-with-c-sharp
    /// </summary>
    [Flags]
    internal enum DisplayDeviceStateFlags : uint
    {
        /// <summary>
        /// The device is part of the desktop.
        /// </summary>
        AttachedToDesktop = 0x1,

        /// <summary>
        /// The primary desktop is on the device.
        /// </summary>
        MultiDriver = 0x2,

        /// <summary>
        /// The device is part of the desktop.
        /// </summary>
        PrimaryDevice = 0x4,

        /// <summary>
        /// Represents a pseudo device used to mirror application drawing for remoting or other purposes.
        /// </summary>
        MirroringDriver = 0x8,

        /// <summary>
        /// The device is VGA compatible.
        /// </summary>
        VGACompatible = 0x10,

        /// <summary>
        /// The device is removable; it cannot be the primary display.
        /// </summary>
        Removable = 0x20,

        /// <summary>
        /// The device has more display modes than its output devices support.
        /// </summary>
        ModesPruned = 0x8000000,

        /// <summary>
        /// Remote control.
        /// </summary>
        Remote = 0x4000000,

        /// <summary>
        /// Remote control.
        /// </summary>
        Disconnect = 0x2000000
    }
}

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace zDrive.Native
{
    /// <summary>
    /// Monitor changer.
    /// </summary>
    public sealed class MonitorChanger
    {
        /// <summary>
        /// Is prime  monitor
        /// </summary>
        public static bool IsPrime(uint id) => Screen.AllScreens[(int)id].Primary;

        public static void SetAsPrimaryMonitor(uint id)
        {
            var device = new DisplayDevice();
            var deviceMode = new Devmode();
            device.cb = Marshal.SizeOf(device);

            NativeMethods.EnumDisplayDevices(null, id, ref device, 0);
            NativeMethods.EnumDisplaySettings(device.DeviceName, -1, ref deviceMode);
            var offsetX = deviceMode.dmPosition.x;
            var offsetY = deviceMode.dmPosition.y;
            deviceMode.dmPosition.x = 0;
            deviceMode.dmPosition.y = 0;

            NativeMethods.ChangeDisplaySettingsEx(
                device.DeviceName,
                ref deviceMode,
                (IntPtr)null,
                ChangeDisplaySettingsFlags.CdsSetPrimary | ChangeDisplaySettingsFlags.CdsUpdateregistry |
                ChangeDisplaySettingsFlags.CdsNoreset,
                IntPtr.Zero);

            device = new DisplayDevice();
            device.cb = Marshal.SizeOf(device);

            // Update remaining devices
            for (uint otherId = 0; NativeMethods.EnumDisplayDevices(null, otherId, ref device, 0); otherId++)
            {
                if (device.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop) && otherId != id)
                {
                    device.cb = Marshal.SizeOf(device);
                    var otherDeviceMode = new Devmode();

                    NativeMethods.EnumDisplaySettings(device.DeviceName, -1, ref otherDeviceMode);

                    otherDeviceMode.dmPosition.x -= offsetX;
                    otherDeviceMode.dmPosition.y -= offsetY;

                    NativeMethods.ChangeDisplaySettingsEx(
                        device.DeviceName,
                        ref otherDeviceMode,
                        (IntPtr)null,
                        ChangeDisplaySettingsFlags.CdsUpdateregistry | ChangeDisplaySettingsFlags.CdsNoreset,
                        IntPtr.Zero);
                }

                device.cb = Marshal.SizeOf(device);
            }

            // Apply settings
            NativeMethods.ChangeDisplaySettingsEx(null, IntPtr.Zero, (IntPtr)null, ChangeDisplaySettingsFlags.CdsNone,
                (IntPtr)null);
        }
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public struct Devmode
    {
        public const int Cchdevicename = 32;
        public const int Cchformname = 32;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Cchdevicename)]
        [FieldOffset(0)]
        public string dmDeviceName;

        [FieldOffset(32)]
        public short dmSpecVersion;

        [FieldOffset(34)]
        public short dmDriverVersion;

        [FieldOffset(36)]
        public short dmSize;

        [FieldOffset(38)]
        public short dmDriverExtra;

        [FieldOffset(40)]
        public uint dmFields;

        [FieldOffset(44)]
        private readonly short dmOrientation;

        [FieldOffset(46)]
        private readonly short dmPaperSize;

        [FieldOffset(48)]
        private readonly short dmPaperLength;

        [FieldOffset(50)]
        private readonly short dmPaperWidth;

        [FieldOffset(52)]
        private readonly short dmScale;

        [FieldOffset(54)]
        private readonly short dmCopies;

        [FieldOffset(56)]
        private readonly short dmDefaultSource;

        [FieldOffset(58)]
        private readonly short dmPrintQuality;

        [FieldOffset(44)]
        public Pointl dmPosition;

        [FieldOffset(52)]
        public int dmDisplayOrientation;

        [FieldOffset(56)]
        public int dmDisplayFixedOutput;

        [FieldOffset(60)]
        public short dmColor; // See note below!

        [FieldOffset(62)]
        public short dmDuplex; // See note below!

        [FieldOffset(64)]
        public short dmYResolution;

        [FieldOffset(66)]
        public short dmTTOption;

        [FieldOffset(68)]
        public short dmCollate; // See note below!

        [FieldOffset(72)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Cchformname)]
        public string dmFormName;

        [FieldOffset(102)]
        public short dmLogPixels;

        [FieldOffset(104)]
        public int dmBitsPerPel;

        [FieldOffset(108)]
        public int dmPelsWidth;

        [FieldOffset(112)]
        public int dmPelsHeight;

        [FieldOffset(116)]
        public int dmDisplayFlags;

        [FieldOffset(116)]
        public int dmNup;

        [FieldOffset(120)]
        public int dmDisplayFrequency;
    }

    public enum DispChange
    {
        Successful = 0,
        Restart = 1,
        Failed = -1,
        BadMode = -2,
        NotUpdated = -3,
        BadFlags = -4,
        BadParam = -5,
        BadDualView = -6
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DisplayDevice
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cb;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;

        [MarshalAs(UnmanagedType.U4)]
        public DisplayDeviceStateFlags StateFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;
    }

    [Flags]
    public enum DisplayDeviceStateFlags
    {
        /// <summary>The device is part of the desktop.</summary>
        AttachedToDesktop = 0x1,
        MultiDriver = 0x2,

        /// <summary>The device is part of the desktop.</summary>
        PrimaryDevice = 0x4,

        /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
        MirroringDriver = 0x8,

        /// <summary>The device is VGA compatible.</summary>
        VgaCompatible = 0x10,

        /// <summary>The device is removable; it cannot be the primary display.</summary>
        Removable = 0x20,

        /// <summary>The device has more display modes than its output devices support.</summary>
        ModesPruned = 0x8000000,
        Remote = 0x4000000,
        Disconnect = 0x2000000
    }

    [Flags]
    public enum ChangeDisplaySettingsFlags : uint
    {
        CdsNone = 0,
        CdsUpdateregistry = 0x00000001,
        CdsTest = 0x00000002,
        CdsFullscreen = 0x00000004,
        CdsGlobal = 0x00000008,
        CdsSetPrimary = 0x00000010,
        CdsVideoparameters = 0x00000020,
        CdsEnableUnsafeModes = 0x00000100,
        CdsDisableUnsafeModes = 0x00000200,
        CdsReset = 0x40000000,
        CdsResetEx = 0x20000000,
        CdsNoreset = 0x10000000
    }

    public class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern DispChange ChangeDisplaySettingsEx(string lpszDeviceName, ref Devmode lpDevMode,
            IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        // A signature for ChangeDisplaySettingsEx with a DEVMODE struct as the second parameter won't allow you to pass in IntPtr.Zero, so create an overload
        public static extern DispChange ChangeDisplaySettingsEx(string lpszDeviceName, IntPtr lpDevMode, IntPtr hwnd,
            ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice,
            uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref Devmode devMode);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Pointl
    {
        public int x;
        public int y;
    }
}

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace zDrive.Native
{
    public sealed class MonitorChanger
    {
        public static bool IsPrime(uint id)
        {
            return Screen.AllScreens[(int) id].Primary;
        }

        public static void SetAsPrimaryMonitor(uint id)
        {
            var device = new DISPLAY_DEVICE();
            var deviceMode = new DEVMODE();
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
                (IntPtr) null,
                ChangeDisplaySettingsFlags.CDS_SET_PRIMARY | ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY |
                ChangeDisplaySettingsFlags.CDS_NORESET,
                IntPtr.Zero);

            device = new DISPLAY_DEVICE();
            device.cb = Marshal.SizeOf(device);

            // Update remaining devices
            for (uint otherId = 0; NativeMethods.EnumDisplayDevices(null, otherId, ref device, 0); otherId++)
            {
                if (device.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop) && otherId != id)
                {
                    device.cb = Marshal.SizeOf(device);
                    var otherDeviceMode = new DEVMODE();

                    NativeMethods.EnumDisplaySettings(device.DeviceName, -1, ref otherDeviceMode);

                    otherDeviceMode.dmPosition.x -= offsetX;
                    otherDeviceMode.dmPosition.y -= offsetY;

                    NativeMethods.ChangeDisplaySettingsEx(
                        device.DeviceName,
                        ref otherDeviceMode,
                        (IntPtr) null,
                        ChangeDisplaySettingsFlags.CDS_UPDATEREGISTRY | ChangeDisplaySettingsFlags.CDS_NORESET,
                        IntPtr.Zero);
                }

                device.cb = Marshal.SizeOf(device);
            }

            // Apply settings
            NativeMethods.ChangeDisplaySettingsEx(null, IntPtr.Zero, (IntPtr) null, ChangeDisplaySettingsFlags.CDS_NONE,
                (IntPtr) null);
        }
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    public struct DEVMODE
    {
        public const int CCHDEVICENAME = 32;
        public const int CCHFORMNAME = 32;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)] [FieldOffset(0)]
        public string dmDeviceName;

        [FieldOffset(32)] public short dmSpecVersion;
        [FieldOffset(34)] public short dmDriverVersion;
        [FieldOffset(36)] public short dmSize;
        [FieldOffset(38)] public short dmDriverExtra;
        [FieldOffset(40)] public uint dmFields;

        [FieldOffset(44)] private readonly short dmOrientation;
        [FieldOffset(46)] private readonly short dmPaperSize;
        [FieldOffset(48)] private readonly short dmPaperLength;
        [FieldOffset(50)] private readonly short dmPaperWidth;
        [FieldOffset(52)] private readonly short dmScale;
        [FieldOffset(54)] private readonly short dmCopies;
        [FieldOffset(56)] private readonly short dmDefaultSource;
        [FieldOffset(58)] private readonly short dmPrintQuality;

        [FieldOffset(44)] public POINTL dmPosition;
        [FieldOffset(52)] public int dmDisplayOrientation;
        [FieldOffset(56)] public int dmDisplayFixedOutput;

        [FieldOffset(60)] public short dmColor; // See note below!
        [FieldOffset(62)] public short dmDuplex; // See note below!
        [FieldOffset(64)] public short dmYResolution;
        [FieldOffset(66)] public short dmTTOption;
        [FieldOffset(68)] public short dmCollate; // See note below!

        [FieldOffset(72)] [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
        public string dmFormName;

        [FieldOffset(102)] public short dmLogPixels;
        [FieldOffset(104)] public int dmBitsPerPel;
        [FieldOffset(108)] public int dmPelsWidth;
        [FieldOffset(112)] public int dmPelsHeight;
        [FieldOffset(116)] public int dmDisplayFlags;
        [FieldOffset(116)] public int dmNup;
        [FieldOffset(120)] public int dmDisplayFrequency;
    }

    public enum DISP_CHANGE
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
    public struct DISPLAY_DEVICE
    {
        [MarshalAs(UnmanagedType.U4)] public int cb;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;

        [MarshalAs(UnmanagedType.U4)] public DisplayDeviceStateFlags StateFlags;

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
        VGACompatible = 0x10,

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
        CDS_NONE = 0,
        CDS_UPDATEREGISTRY = 0x00000001,
        CDS_TEST = 0x00000002,
        CDS_FULLSCREEN = 0x00000004,
        CDS_GLOBAL = 0x00000008,
        CDS_SET_PRIMARY = 0x00000010,
        CDS_VIDEOPARAMETERS = 0x00000020,
        CDS_ENABLE_UNSAFE_MODES = 0x00000100,
        CDS_DISABLE_UNSAFE_MODES = 0x00000200,
        CDS_RESET = 0x40000000,
        CDS_RESET_EX = 0x20000000,
        CDS_NORESET = 0x10000000
    }

    public class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern DISP_CHANGE ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode,
            IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        // A signature for ChangeDisplaySettingsEx with a DEVMODE struct as the second parameter won't allow you to pass in IntPtr.Zero, so create an overload
        public static extern DISP_CHANGE ChangeDisplaySettingsEx(string lpszDeviceName, IntPtr lpDevMode, IntPtr hwnd,
            ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice,
            uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        public int x;
        public int y;
    }
}
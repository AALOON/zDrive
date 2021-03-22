using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace zDrive.Native
{
    /// <summary>
    /// Monitor changer.
    /// </summary>
    internal sealed class MonitorChanger
    {
        /// <summary>
        /// Is prime  monitor
        /// </summary>
        internal static bool IsPrime(uint id) => Screen.AllScreens[(int)id].Primary;

        internal static void SetAsPrimaryMonitor(uint id)
        {
            var device = new DisplayDevice();
            var deviceMode = new Devmode();
            device.Cb = Marshal.SizeOf(device);

            NativeMethods.EnumDisplayDevices(null, id, ref device, 0);
            NativeMethods.EnumDisplaySettings(device.DeviceName, -1, ref deviceMode);
            var offsetX = deviceMode.DmPosition.X;
            var offsetY = deviceMode.DmPosition.Y;
            deviceMode.DmPosition.X = 0;
            deviceMode.DmPosition.Y = 0;

            NativeMethods.ChangeDisplaySettingsEx(
                device.DeviceName,
                ref deviceMode,
                (IntPtr)null,
                ChangeDisplaySettingsFlags.CdsSetPrimary | ChangeDisplaySettingsFlags.CdsUpdateregistry |
                ChangeDisplaySettingsFlags.CdsNoreset,
                IntPtr.Zero);

            device = new DisplayDevice();
            device.Cb = Marshal.SizeOf(device);

            // Update remaining devices
            for (uint otherId = 0; NativeMethods.EnumDisplayDevices(null, otherId, ref device, 0); otherId++)
            {
                if (device.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop) && otherId != id)
                {
                    device.Cb = Marshal.SizeOf(device);
                    var otherDeviceMode = new Devmode();

                    NativeMethods.EnumDisplaySettings(device.DeviceName, -1, ref otherDeviceMode);

                    otherDeviceMode.DmPosition.X -= offsetX;
                    otherDeviceMode.DmPosition.Y -= offsetY;

                    NativeMethods.ChangeDisplaySettingsEx(
                        device.DeviceName,
                        ref otherDeviceMode,
                        (IntPtr)null,
                        ChangeDisplaySettingsFlags.CdsUpdateregistry | ChangeDisplaySettingsFlags.CdsNoreset,
                        IntPtr.Zero);
                }

                device.Cb = Marshal.SizeOf(device);
            }

            // Apply settings
            NativeMethods.ChangeDisplaySettingsEx(null, IntPtr.Zero, (IntPtr)null, ChangeDisplaySettingsFlags.CdsNone,
                (IntPtr)null);
        }
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    internal struct Devmode
    {
        internal const int Cchdevicename = 32;
        internal const int Cchformname = 32;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Cchdevicename)]
        [FieldOffset(0)]
        internal string DmDeviceName;

        [FieldOffset(32)]
        internal short DmSpecVersion;

        [FieldOffset(34)]
        internal short DmDriverVersion;

        [FieldOffset(36)]
        internal short DmSize;

        [FieldOffset(38)]
        internal short DmDriverExtra;

        [FieldOffset(40)]
        internal uint DmFields;

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
        internal Pointl DmPosition;

        [FieldOffset(52)]
        internal int DmDisplayOrientation;

        [FieldOffset(56)]
        internal int DmDisplayFixedOutput;

        [FieldOffset(60)]
        internal short DmColor; // See note below!

        [FieldOffset(62)]
        internal short DmDuplex; // See note below!

        [FieldOffset(64)]
        internal short DmYResolution;

        [FieldOffset(66)]
        internal short DmTTOption;

        [FieldOffset(68)]
        internal short DmCollate; // See note below!

        [FieldOffset(72)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Cchformname)]
        internal string DmFormName;

        [FieldOffset(102)]
        internal short DmLogPixels;

        [FieldOffset(104)]
        internal int DmBitsPerPel;

        [FieldOffset(108)]
        internal int DmPelsWidth;

        [FieldOffset(112)]
        internal int DmPelsHeight;

        [FieldOffset(116)]
        internal int DmDisplayFlags;

        [FieldOffset(116)]
        internal int DmNup;

        [FieldOffset(120)]
        internal int DmDisplayFrequency;
    }

    internal enum DispChange
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
    internal struct DisplayDevice
    {
        [MarshalAs(UnmanagedType.U4)]
        internal int Cb;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        internal string DeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string DeviceString;

        [MarshalAs(UnmanagedType.U4)]
        internal DisplayDeviceStateFlags StateFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string DeviceID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string DeviceKey;
    }

    [Flags]
    internal enum DisplayDeviceStateFlags
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
    internal enum ChangeDisplaySettingsFlags : uint
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

    internal class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern DispChange ChangeDisplaySettingsEx(string lpszDeviceName, ref Devmode lpDevMode,
            IntPtr hwnd, ChangeDisplaySettingsFlags dwFlags, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        // A signature for ChangeDisplaySettingsEx with a DEVMODE struct as the second parameter won't allow you to pass in IntPtr.Zero, so create an overload
        internal static extern DispChange ChangeDisplaySettingsEx(string lpszDeviceName, IntPtr lpDevMode, IntPtr hwnd,
            ChangeDisplaySettingsFlags dwFlags, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice,
            uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref Devmode devMode);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Pointl
    {
        internal int X;
        internal int Y;
    }
}

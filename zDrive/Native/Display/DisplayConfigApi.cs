using System;
using System.Runtime.InteropServices;
using zDrive.Native.Display.DisplayConfig;
using zDrive.Native.Display.DisplayConfig.Structures;

namespace zDrive.Native.Display
{
    /// <summary>
    /// Windows/Apps/Win32/API/Display Devices Reference/Winuser.h/
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/
    /// </summary>
    internal static class DisplayConfigApi
    {
        private const string User32 = "user32.dll";

        /// <summary>
        /// The DisplayConfigGetDeviceInfo function retrieves display configuration information about the device.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfiggetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigGetDeviceInfo(
            ref DisplayConfigSupportVirtualResolution targetSupportVirtualResolution
        );

        /// <summary>
        /// The DisplayConfigGetDeviceInfo function retrieves display configuration information about the device.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfiggetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigGetDeviceInfo(
            ref DisplayConfigGetSourceDPIScale targetSupportVirtualResolution
        );

        /// <summary>
        /// The DisplayConfigGetDeviceInfo function retrieves display configuration information about the device.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfiggetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigGetDeviceInfo(
            ref DisplayConfigTargetDeviceName deviceName
        );

        /// <summary>
        /// The DisplayConfigGetDeviceInfo function retrieves display configuration information about the device.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfiggetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigGetDeviceInfo(
            ref DisplayConfigAdapterName deviceName
        );

        /// <summary>
        /// The DisplayConfigGetDeviceInfo function retrieves display configuration information about the device.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfiggetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigGetDeviceInfo(
            ref DisplayConfigSourceDeviceName deviceName
        );

        /// <summary>
        /// The DisplayConfigGetDeviceInfo function retrieves display configuration information about the device.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfiggetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigGetDeviceInfo(
            ref DisplayConfigTargetPreferredMode targetPreferredMode
        );

        /// <summary>
        /// The DisplayConfigGetDeviceInfo function retrieves display configuration information about the device.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfiggetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigGetDeviceInfo(
            ref DisplayConfigTargetBaseType targetBaseType
        );

        /// <summary>
        /// The DisplayConfigSetDeviceInfo function sets the properties of a target.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfigsetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigSetDeviceInfo(
            ref DisplayConfigSetTargetPersistence targetPersistence
        );

        /// <summary>
        /// The DisplayConfigSetDeviceInfo function sets the properties of a target.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfigsetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigSetDeviceInfo(
            ref DisplayConfigSupportVirtualResolution targetSupportVirtualResolution
        );

        /// <summary>
        /// The DisplayConfigSetDeviceInfo function sets the properties of a target.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-displayconfigsetdeviceinfo
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status DisplayConfigSetDeviceInfo(
            ref DisplayConfigSetSourceDPIScale setSourceDpiScale
        );

        /// <summary>
        /// The GetDisplayConfigBufferSizes function retrieves the size of the buffers that are required to call the
        /// QueryDisplayConfig function.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdisplayconfigbuffersizes
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status GetDisplayConfigBufferSizes(
            QueryDeviceConfigFlags flags,
            out uint pathArrayElements,
            out uint modeInfoArrayElements
        );

        /// <summary>
        /// The QueryDisplayConfig function retrieves information about all possible display paths for all display devices, or
        /// views, in the current setting.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-querydisplayconfig
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status QueryDisplayConfig(
            QueryDeviceConfigFlags flags,
            ref uint pathArrayElements,
            [Out] DisplayConfigPathInfo[] pathInfoArray,
            ref uint modeInfoArrayElements,
            [Out] DisplayConfigModeInfo[] modeInfoArray,
            IntPtr currentTopologyId
        );

        /// <summary>
        /// The QueryDisplayConfig function retrieves information about all possible display paths for all display devices, or
        /// views, in the current setting.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-querydisplayconfig
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status QueryDisplayConfig(
            QueryDeviceConfigFlags flags,
            ref uint pathArrayElements,
            [Out] DisplayConfigPathInfo[] pathInfoArray,
            ref uint modeInfoArrayElements,
            [Out] DisplayConfigModeInfo[] modeInfoArray,
            [Out] out DisplayConfigTopologyId currentTopologyId
        );

        /// <summary>
        /// The SetDisplayConfig function modifies the display topology, source, and target modes by exclusively enabling the
        /// specified paths in the current session.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setdisplayconfig
        /// </summary>
        [DllImport(User32)]
        public static extern User32Status SetDisplayConfig(
            [In] uint pathArrayElements,
            [In] DisplayConfigPathInfo[] pathInfoArray,
            [In] uint modeInfoArrayElements,
            [In] DisplayConfigModeInfo[] modeInfoArray,
            [In] SetDisplayConfigFlags flags
        );
    }
}

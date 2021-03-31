using System;
using System.Runtime.InteropServices;
using zDrive.Native.Display.DeviceContext;
using zDrive.Native.Display.DeviceContext.Structures;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display
{
    /// <summary>
    /// Windows/Apps/Win32/API/Windows GDI/Winuser.h/
    /// https://docs.microsoft.com/en-us/windows/win32/api/_gdi/
    /// </summary>
    internal static class DisplayContextApi
    {
        private const string User32 = "user32.dll";
        private const string Gdi32 = "gdi.dll";

        /// <summary>
        /// The ChangeDisplaySettingsEx function changes the settings of the specified display device to the specified graphics
        /// mode.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changedisplaysettingsexa
        /// </summary>
        [DllImport(User32, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern ChangeDisplaySettingsExResult ChangeDisplaySettingsEx(
            string deviceName,
            ref DeviceMode devMode,
            IntPtr handler,
            ChangeDisplaySettingsFlags flags,
            IntPtr param
        );

        /// <summary>
        /// The ChangeDisplaySettingsEx function changes the settings of the specified display device to the specified graphics
        /// mode.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changedisplaysettingsexa
        /// </summary>
        [DllImport(User32, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern ChangeDisplaySettingsExResult ChangeDisplaySettingsEx(
            string deviceName,
            IntPtr devModePointer,
            IntPtr handler,
            ChangeDisplaySettingsFlags flags,
            IntPtr param
        );

        /// <summary>
        /// The EnumDisplaySettings function retrieves information about one of the graphics modes for a display device. To
        /// retrieve information for all the graphics modes of a display device, make a series of calls to this function.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdisplaysettingsa
        /// </summary>
        [DllImport(User32, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern bool EnumDisplaySettings(
            string deviceName,
            DisplaySettingsMode mode,
            ref DeviceMode devMode
        );

        /// <summary>
        /// The CreateDC function creates a device context (DC) for a device using the specified name.
        /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-createdca
        /// </summary>
        [DllImport(Gdi32, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateDC(string driver, string device, string port, IntPtr deviceMode);

        /// <summary>
        /// The DeleteDC function deletes the specified device context (DC).
        /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-deletedc
        /// </summary>
        [DllImport(Gdi32)]
        internal static extern bool DeleteDC(DeviceContextHandle deviceContextHandle);

        /// <summary>
        /// The EnumDisplayDevices function lets you obtain information about the display devices in the current session.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdisplaydevicesa
        /// </summary>
        [DllImport(User32, CharSet = CharSet.Unicode)]
        internal static extern bool EnumDisplayDevices(
            string deviceName,
            uint deviceNumber,
            ref DisplayDevice displayDevice,
            uint flags
        );

        /// <summary>
        /// The EnumDisplayMonitors function enumerates display monitors (including invisible pseudo-monitors associated with the
        /// mirroring drivers) that intersect a region formed by the intersection of a specified clipping rectangle and the visible
        /// region of a device context. EnumDisplayMonitors calls an application-defined MonitorEnumProc callback function once for
        /// each monitor that is enumerated. Note that GetSystemMetrics (SM_CMONITORS) counts only the display monitors.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enumdisplaymonitors
        /// </summary>
        [DllImport(User32)]
        internal static extern bool EnumDisplayMonitors(
            [In] IntPtr dcHandle,
            [In] IntPtr clip,
            MonitorEnumProcedure func,
            IntPtr callbackObject
        );

        /// <summary>
        /// The GetDC function retrieves a handle to a device context (DC) for the client area of a specified window or for the
        /// entire screen. You can use the returned handle in subsequent GDI functions to draw in the DC. The device context is an
        /// opaque data structure, whose values are used internally by GDI.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdc
        /// </summary>
        [DllImport(User32)]
        internal static extern IntPtr GetDC(IntPtr windowHandle);

        /// <summary>
        /// The GetDeviceCaps function retrieves device-specific information for the specified device.
        /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-getdevicecaps
        /// </summary>
        [DllImport(Gdi32)]
        internal static extern int GetDeviceCaps(DeviceContextHandle deviceContextHandle, DeviceCapability index);

        /// <summary>
        /// The GetDeviceGammaRamp function gets the gamma ramp on direct color display boards having drivers that support
        /// downloadable gamma ramps in hardware.
        /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-getdevicegammaramp
        /// </summary>
        [DllImport(Gdi32)]
        internal static extern bool GetDeviceGammaRamp(DeviceContextHandle deviceContextHandle, ref GammaRamp ramp);

        /// <summary>
        /// The GetMonitorInfo function retrieves information about a display monitor.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getmonitorinfoa
        /// </summary>
        [DllImport(User32)]
        internal static extern bool GetMonitorInfo(
            IntPtr monitorHandle,
            ref DeviceContext.Structures.MonitorInfo monitorInfo
        );

        /// <summary>
        /// The MonitorFromPoint function retrieves a handle to the display monitor that contains a specified point.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-monitorfrompoint
        /// </summary>
        [DllImport(User32)]
        internal static extern IntPtr MonitorFromPoint(
            [In] PointL point,
            MonitorFromFlag flag
        );

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-monitorfromrect
        /// The MonitorFromRect function retrieves a handle to the display monitor that has the largest area of intersection with a
        /// specified rectangle.
        /// </summary>
        [DllImport(User32)]
        internal static extern IntPtr MonitorFromRect(
            [In] RectangleL rectangle,
            MonitorFromFlag flag
        );

        /// <summary>
        /// The MonitorFromWindow function retrieves a handle to the display monitor that has the largest area of intersection with
        /// the bounding rectangle of a specified window.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-monitorfromwindow
        /// </summary>
        [DllImport(User32)]
        internal static extern IntPtr MonitorFromWindow(
            [In] IntPtr windowHandle,
            MonitorFromFlag flag
        );

        /// <summary>
        /// The ReleaseDC function releases a device context (DC), freeing it for use by other applications. The effect of the
        /// ReleaseDC function depends on the type of DC. It frees only common and window DCs. It has no effect on class or private
        /// DCs.
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-releasedc
        /// </summary>
        [DllImport(User32)]
        internal static extern bool ReleaseDC([In] IntPtr windowHandle, [In] DeviceContextHandle deviceContextHandle);

        /// <summary>
        /// The SetDeviceGammaRamp function sets the gamma ramp on direct color display boards having drivers that support
        /// downloadable gamma ramps in hardware.
        /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-setdevicegammaramp
        /// </summary>
        /// <remarks>
        /// WARNING! Don't use this method until you read  documentation.
        /// </remarks>
        [DllImport(Gdi32)]
        internal static extern bool SetDeviceGammaRamp(DeviceContextHandle deviceContextHandle, ref GammaRamp ramp);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate int MonitorEnumProcedure(
            IntPtr monitorHandle,
            IntPtr dcHandle,
            ref RectangleL rect,
            IntPtr callbackObject
        );
    }
}

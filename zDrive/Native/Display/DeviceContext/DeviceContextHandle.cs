using System;
using System.Runtime.InteropServices;

namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// Handle for device context.
    /// Uses CreateDC and other functions of gdi32.
    /// </summary>
    internal sealed class DeviceContextHandle : SafeHandle
    {
        private readonly bool created;

        private DeviceContextHandle(IntPtr handle, bool created) : base(handle, true) => this.created = created;

        public override bool IsInvalid => this.handle == IntPtr.Zero;

        public static DeviceContextHandle Create() =>
            new(
                DisplayContextApi.CreateDC("DISPLAY", null, null, IntPtr.Zero),
                true
            );

        public static DeviceContextHandle CreateFromDevice(string screenName, string devicePath) =>
            new(
                DisplayContextApi.CreateDC(screenName, devicePath, null, IntPtr.Zero),
                true
            );

        public static DeviceContextHandle CreateFromScreen(string screenName) =>
            CreateFromDevice(screenName, screenName);

        public static DeviceContextHandle CreateFromWindowHandle(IntPtr windowHandle) =>
            new(
                DisplayContextApi.GetDC(windowHandle),
                true
            );

        protected override bool ReleaseHandle() =>
            this.created
                ? DisplayContextApi.DeleteDC(this)
                : DisplayContextApi.ReleaseDC(IntPtr.Zero, this);
    }
}

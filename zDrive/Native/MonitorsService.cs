using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using zDrive.Native.Display;
using zDrive.Native.Display.DeviceContext;
using zDrive.Native.Display.DeviceContext.Structures;
using zDrive.Native.Display.DisplayConfig;
using zDrive.Native.Display.DisplayConfig.Structures;
using zDrive.Native.Display.Structures;

namespace zDrive.Native
{
    public sealed class MonitorsService
    {
        private const int ErrorSuccess = 0;

        private const string DeviceNamePrefix = @"\\.\";

        private static readonly Lazy<MonitorsService> LazyInstance = new(() => new MonitorsService());

        public static MonitorsService Instance { get; } = LazyInstance.Value;

        public IEnumerable<MonitorInfo> AllMonitorDevices()
        {
            var targets = this.GetAllDisplayTargets();
            var screens = Screen.AllScreens;
            for (var i = 0; i < screens.Length; i++)
            {
                var screen = screens[i];
                var target = this.FindTarget(targets, screen.DeviceName);
                var isHdrSupported = false;
                var isHdrEnabled = false;
                if (target != null && TryGetAdvancedColorInfo(target.AdapterId, target.TargetId, out var colorInfo))
                {
                    isHdrSupported = colorInfo.IsAdvancedColorSupported;
                    isHdrEnabled = colorInfo.IsAdvancedColorEnabled;
                }

                yield return new MonitorInfo((uint)i, screen.Primary, screen.DeviceName,
                    target?.FriendlyName ?? screen.DeviceName, isHdrSupported, isHdrEnabled);
            }
        }

        public string DeviceFriendlyName(Screen screen) =>
            this.FindTarget(this.GetAllDisplayTargets(), screen.DeviceName)?.FriendlyName ?? screen.DeviceName;

        public string DeviceFriendlyName(uint screenId)
        {
            var screens = Screen.AllScreens;
            if (screens.Length <= screenId)
            {
                throw new ArgumentOutOfRangeException(nameof(screenId));
            }

            return this.DeviceFriendlyName(screens[screenId]);
        }

        /// <summary>
        /// Hdr state of the display.
        /// </summary>
        public (bool IsSupported, bool IsEnabled) GetHdrState(string deviceName)
        {
            var target = this.FindTarget(this.GetAllDisplayTargets(), deviceName);
            if (target == null || !TryGetAdvancedColorInfo(target.AdapterId, target.TargetId, out var colorInfo))
            {
                return (false, false);
            }

            return (colorInfo.IsAdvancedColorSupported, colorInfo.IsAdvancedColorEnabled);
        }

        /// <summary>
        /// Is hdr enabled on the display.
        /// </summary>
        public bool IsHdrEnabled(string deviceName) => this.GetHdrState(deviceName).IsEnabled;

        /// <summary>
        /// Enable or disable hdr on the display.
        /// </summary>
        public void SetHdr(string deviceName, bool enable)
        {
            var target = this.FindTarget(this.GetAllDisplayTargets(), deviceName);
            if (target == null)
            {
                throw new ArgumentException($"Display '{deviceName}' is not found.", nameof(deviceName));
            }

            var state = new DisplayConfigSetAdvancedColorInfo(target.AdapterId, target.TargetId, enable);
            var error = DisplayConfigApi.DisplayConfigSetDeviceInfo(ref state);
            if (error != ErrorSuccess)
            {
                throw new Win32Exception((int)error);
            }
        }

        /// <summary>
        /// Is prime monitor.
        /// </summary>
        internal bool IsPrime(uint id) => Screen.AllScreens[(int)id].Primary;

        internal void SetAsPrimaryMonitor(uint id)
        {
            var device = DisplayDevice.Initialize();
            var deviceMode = new DeviceMode();

            DisplayContextApi.EnumDisplayDevices(null, id, ref device, 0);
            DisplayContextApi.EnumDisplaySettings(device.DeviceName, DisplaySettingsMode.CurrentSettings,
                ref deviceMode);
            var offsetX = deviceMode.Position.X;
            var offsetY = deviceMode.Position.Y;
            deviceMode.Position = new PointL(0, 0);

            DisplayContextApi.ChangeDisplaySettingsEx(
                device.DeviceName,
                ref deviceMode,
                (IntPtr)null,
                ChangeDisplaySettingsFlags.SetPrimary | ChangeDisplaySettingsFlags.UpdateRegistry |
                ChangeDisplaySettingsFlags.NoReset,
                IntPtr.Zero);

            device = DisplayDevice.Initialize();

            // Update remaining devices
            for (uint otherId = 0; DisplayContextApi.EnumDisplayDevices(null, otherId, ref device, 0); otherId++)
            {
                if (device.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop) && otherId != id)
                {
                    device.Size = (uint)Marshal.SizeOf(typeof(DisplayDevice));
                    var otherDeviceMode = new DeviceMode();

                    DisplayContextApi.EnumDisplaySettings(device.DeviceName, DisplaySettingsMode.CurrentSettings,
                        ref otherDeviceMode);

                    otherDeviceMode.Position.X -= offsetX;
                    otherDeviceMode.Position.Y -= offsetY;

                    DisplayContextApi.ChangeDisplaySettingsEx(
                        device.DeviceName,
                        ref otherDeviceMode,
                        (IntPtr)null,
                        ChangeDisplaySettingsFlags.UpdateRegistry | ChangeDisplaySettingsFlags.NoReset,
                        IntPtr.Zero);
                }

                device = DisplayDevice.Initialize();
            }

            // Apply settings
            DisplayContextApi.ChangeDisplaySettingsEx(null, IntPtr.Zero, (IntPtr)null, ChangeDisplaySettingsFlags.None,
                (IntPtr)null);
        }

        private IEnumerable<DisplayTarget> GetAllDisplayTargets()
        {
            var error = DisplayConfigApi.GetDisplayConfigBufferSizes(QueryDeviceConfigFlags.OnlyActivePaths,
                out var pathCount,
                out var modeCount);
            if (error != ErrorSuccess)
            {
                throw new Win32Exception((int)error);
            }

            var displayPaths = new DisplayConfigPathInfo[pathCount];
            var displayModes = new DisplayConfigModeInfo[modeCount];
            error = DisplayConfigApi.QueryDisplayConfig(QueryDeviceConfigFlags.OnlyActivePaths,
                ref pathCount, displayPaths, ref modeCount, displayModes, IntPtr.Zero);
            if (error != ErrorSuccess)
            {
                throw new Win32Exception((int)error);
            }

            for (var i = 0; i < pathCount; i++)
            {
                var path = displayPaths[i];
                var sourceName = new DisplayConfigSourceDeviceName(path.SourceInfo.AdapterId,
                    path.SourceInfo.SourceId);
                error = DisplayConfigApi.DisplayConfigGetDeviceInfo(ref sourceName);
                if (error != ErrorSuccess)
                {
                    throw new Win32Exception((int)error);
                }

                yield return new DisplayTarget(sourceName.DeviceName,
                    MonitorFriendlyName(path.TargetInfo.AdapterId, path.TargetInfo.TargetId),
                    path.TargetInfo.AdapterId, path.TargetInfo.TargetId);
            }
        }

        private DisplayTarget FindTarget(IEnumerable<DisplayTarget> targets, string deviceName)
        {
            var normalized = NormalizeDeviceName(deviceName);
            return targets.FirstOrDefault(target =>
                string.Equals(target.DeviceName, normalized, StringComparison.OrdinalIgnoreCase));
        }

        private static bool TryGetAdvancedColorInfo(Luid adapterId, uint targetId,
            out DisplayConfigGetAdvancedColorInfo info)
        {
            info = new DisplayConfigGetAdvancedColorInfo(adapterId, targetId);
            return DisplayConfigApi.DisplayConfigGetDeviceInfo(ref info) == ErrorSuccess;
        }

        private static string NormalizeDeviceName(string deviceName) =>
            deviceName.StartsWith(DeviceNamePrefix, StringComparison.OrdinalIgnoreCase)
                ? deviceName
                : DeviceNamePrefix + deviceName;

        private static string MonitorFriendlyName(Luid adapterId, uint targetId)
        {
            var deviceName = new DisplayConfigTargetDeviceName(adapterId, targetId);
            var error = DisplayConfigApi.DisplayConfigGetDeviceInfo(ref deviceName);
            if (error != ErrorSuccess)
            {
                throw new Win32Exception((int)error);
            }

            return deviceName.MonitorFriendlyDeviceName;
        }

        private sealed class DisplayTarget
        {
            public DisplayTarget(string deviceName, string friendlyName, Luid adapterId, uint targetId)
            {
                this.DeviceName = deviceName;
                this.FriendlyName = friendlyName;
                this.AdapterId = adapterId;
                this.TargetId = targetId;
            }

            public string DeviceName { get; }

            public string FriendlyName { get; }

            public Luid AdapterId { get; }

            public uint TargetId { get; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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

        private static readonly Lazy<MonitorsService> LazyInstance = new(() => new MonitorsService());

        public static MonitorsService Instance { get; } = LazyInstance.Value;

        public IEnumerable<MonitorInfo> AllMonitorDevices()
        {
            var allFriendlyNames = this.GetAllMonitorsFriendlyNames().ToArray();
            var screens = EnumerateAttachedScreens().ToArray();
            for (var i = 0; i < screens.Length; i++)
            {
                var screen = screens[i];
                yield return new MonitorInfo((uint)i, screen.IsPrimary, screen.DeviceName, allFriendlyNames[i]);
            }
        }

        /// <summary>
        /// Count of screens attached to the desktop
        /// (the same set that WinForms Screen.AllScreens used to return).
        /// </summary>
        public int ScreensCount => EnumerateAttachedScreens().Count();

        public string DeviceFriendlyName(uint screenId)
        {
            var allFriendlyNames = this.GetAllMonitorsFriendlyNames().ToArray();
            if (allFriendlyNames.Length <= screenId)
            {
                throw new ArgumentOutOfRangeException(nameof(screenId));
            }

            return allFriendlyNames[screenId];
        }

        /// <summary>
        /// Is prime monitor.
        /// </summary>
        internal bool IsPrime(uint id) => EnumerateAttachedScreens().ElementAt((int)id).IsPrimary;

        private static IEnumerable<(string DeviceName, bool IsPrimary)> EnumerateAttachedScreens()
        {
            var device = DisplayDevice.Initialize();
            for (uint id = 0; DisplayContextApi.EnumDisplayDevices(null, id, ref device, 0); id++)
            {
                if (device.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                {
                    yield return (device.DeviceName, device.StateFlags.HasFlag(DisplayDeviceStateFlags.PrimaryDevice));
                }

                device = DisplayDevice.Initialize();
            }
        }

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

        private IEnumerable<string> GetAllMonitorsFriendlyNames()
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

            for (var i = 0; i < modeCount; i++)
            {
                if (displayModes[i].InfoType == DisplayConfigModeInfoType.Target)
                {
                    yield return MonitorFriendlyName(displayModes[i].AdapterId, displayModes[i].Id);
                }
            }
        }

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
    }
}

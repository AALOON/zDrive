using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace zDrive.Native
{
    public static class MonitorsService
    {
        private const int ErrorSuccess = 0;

        public static IEnumerable<MonitorInfo> AllMonitorDevices()
        {
            var allFriendlyNames = GetAllMonitorsFriendlyNames().ToArray();
            var screens = Screen.AllScreens;
            for (var i = 0; i < screens.Length; i++)
            {
                var screen = screens[i];
                yield return new MonitorInfo((uint)i, screen.Primary, screen.DeviceName, allFriendlyNames[i]);
            }
        }

        public static string DeviceFriendlyName(this Screen screen)
        {
            var allFriendlyNames = GetAllMonitorsFriendlyNames();
            for (var index = 0; index < Screen.AllScreens.Length; index++)
            {
                if (Equals(screen, Screen.AllScreens[index]))
                {
                    return allFriendlyNames.ToArray()[index];
                }
            }

            return null;
        }

        public static string DeviceFriendlyName(this uint screenId)
        {
            var allFriendlyNames = GetAllMonitorsFriendlyNames().ToArray();
            if (allFriendlyNames.Length <= screenId)
            {
                throw new ArgumentOutOfRangeException(nameof(screenId));
            }

            return allFriendlyNames[screenId];
        }

        private static IEnumerable<string> GetAllMonitorsFriendlyNames()
        {
            var error = GetDisplayConfigBufferSizes(QueryDeviceConfigFlags.QdcOnlyActivePaths, out var pathCount,
                out var modeCount);
            if (error != ErrorSuccess)
            {
                throw new Win32Exception(error);
            }

            var displayPaths = new DisplayconfigPathInfo[pathCount];
            var displayModes = new DisplayconfigModeInfo[modeCount];
            error = QueryDisplayConfig(QueryDeviceConfigFlags.QdcOnlyActivePaths,
                ref pathCount, displayPaths, ref modeCount, displayModes, IntPtr.Zero);
            if (error != ErrorSuccess)
            {
                throw new Win32Exception(error);
            }

            for (var i = 0; i < modeCount; i++)
            {
                if (displayModes[i].infoType == DisplayConfigModeInfoType.Target)
                {
                    yield return MonitorFriendlyName(displayModes[i].adapterId, displayModes[i].id);
                }
            }
        }

        private static string MonitorFriendlyName(Luid adapterId, uint targetId)
        {
            var deviceName = new DisplayconfigTargetDeviceName
            {
                header =
                {
                    size = (uint)Marshal.SizeOf(typeof(DisplayconfigTargetDeviceName)),
                    adapterId = adapterId,
                    id = targetId,
                    type = DisplayConfigDeviceInfoType.GetTargetName
                }
            };
            var error = DisplayConfigGetDeviceInfo(ref deviceName);
            if (error != ErrorSuccess)
            {
                throw new Win32Exception(error);
            }

            return deviceName.monitorFriendlyDeviceName;
        }

        #region Enums

        private enum QueryDeviceConfigFlags : uint
        {
            QdcAllPaths = 0x00000001,
            QdcOnlyActivePaths = 0x00000002,
            QdcDatabaseCurrent = 0x00000004
        }

        private enum DisplayConfigScanlineOrdering : uint
        {
            DisplayconfigScanlineOrderingUnspecified = 0,
            DisplayconfigScanlineOrderingProgressive = 1,
            DisplayconfigScanlineOrderingInterlaced = 2,
            DisplayconfigScanlineOrderingInterlacedUpperfieldfirst = DisplayconfigScanlineOrderingInterlaced,
            DisplayconfigScanlineOrderingInterlacedLowerfieldfirst = 3,
            DisplayconfigScanlineOrderingForceUint32 = 0xFFFFFFFF
        }

        private enum DisplayconfigRotation : uint
        {
            DisplayconfigRotationIdentity = 1,
            DisplayconfigRotationRotate90 = 2,
            DisplayconfigRotationRotate180 = 3,
            DisplayconfigRotationRotate270 = 4,
            DisplayconfigRotationForceUint32 = 0xFFFFFFFF
        }

        private enum DisplayConfigScaling : uint
        {
            Identity = 1,
            Centered = 2,
            Stretched = 3,
            Aspectratiocenteredmax = 4,
            Custom = 5,
            Preferred = 128,
            ForceUint32 = 0xFFFFFFFF
        }

        private enum DisplayConfigPixelFormat : uint
        {
            Format8Bpp = 1,
            Format16Bpp = 2,
            Format24Bpp = 3,
            Format32Bpp = 4,
            FormatNongdi = 5,
            FormatForceUint32 = 0xffffffff
        }

        private enum DisplayConfigModeInfoType : uint
        {
            Source = 1,
            Target = 2,
            ForceUint32 = 0xFFFFFFFF
        }

        private enum DisplayConfigDeviceInfoType : uint
        {
            FormatGetSourceName = 1,
            GetTargetName = 2,
            FormatGetTargetPreferredMode = 3,
            FormatGetAdapterName = 4,
            FormatSetTargetPersistence = 5,
            FormatGetTargetBaseType = 6,
            FormatForceUint32 = 0xFFFFFFFF
        }

        #endregion

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        private struct Luid
        {
            public readonly uint LowPart;
            public readonly int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigPathSourceInfo
        {
            public readonly Luid adapterId;
            public readonly uint id;
            public readonly uint modeInfoIdx;
            public readonly uint statusFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigPathTargetInfo
        {
            public readonly Luid adapterId;
            public readonly uint id;
            public readonly uint modeInfoIdx;
            private readonly DisplayConfigVideoOutputTechnology outputTechnology;
            private readonly DisplayconfigRotation rotation;
            private readonly DisplayConfigScaling scaling;
            private readonly DisplayconfigRational refreshRate;
            private readonly DisplayConfigScanlineOrdering scanLineOrdering;
            public readonly bool targetAvailable;
            public readonly uint statusFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigRational
        {
            public readonly uint Numerator;
            public readonly uint Denominator;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigPathInfo
        {
            public readonly DisplayconfigPathSourceInfo sourceInfo;
            public readonly DisplayconfigPathTargetInfo targetInfo;
            public readonly uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Displayconfig2Dregion
        {
            public readonly uint cx;
            public readonly uint cy;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigVideoSignalInfo
        {
            public readonly ulong pixelRate;
            public readonly DisplayconfigRational hSyncFreq;
            public readonly DisplayconfigRational vSyncFreq;
            public readonly Displayconfig2Dregion activeSize;
            public readonly Displayconfig2Dregion totalSize;
            public readonly uint videoStandard;
            public readonly DisplayConfigScanlineOrdering scanLineOrdering;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigTargetMode
        {
            public readonly DisplayconfigVideoSignalInfo targetVideoSignalInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Pointl
        {
            private readonly int x;
            private readonly int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigSourceMode
        {
            public readonly uint width;
            public readonly uint height;
            public readonly DisplayConfigPixelFormat pixelFormat;
            public readonly Pointl position;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct DisplayconfigModeInfoUnion
        {
            [FieldOffset(0)]
            public readonly DisplayconfigTargetMode targetMode;

            [FieldOffset(0)]
            public readonly DisplayconfigSourceMode sourceMode;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigModeInfo
        {
            public readonly DisplayConfigModeInfoType infoType;
            public readonly uint id;
            public readonly Luid adapterId;
            public readonly DisplayconfigModeInfoUnion modeInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct DisplayconfigTargetDeviceNameFlags
        {
            public readonly uint value;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FormatHeader
        {
            public DisplayConfigDeviceInfoType type;
            public uint size;
            public Luid adapterId;
            public uint id;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DisplayconfigTargetDeviceName
        {
            public FormatHeader header;
            public readonly DisplayconfigTargetDeviceNameFlags flags;
            public readonly DisplayConfigVideoOutputTechnology outputTechnology;
            public readonly ushort edidManufactureId;
            public readonly ushort edidProductCodeId;
            public readonly uint connectorInstance;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public readonly string monitorFriendlyDeviceName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public readonly string monitorDevicePath;
        }

        #endregion

        #region DLL-Imports

        [DllImport("user32.dll")]
        private static extern int GetDisplayConfigBufferSizes(
            QueryDeviceConfigFlags flags, out uint numPathArrayElements, out uint numModeInfoArrayElements);

        [DllImport("user32.dll")]
        private static extern int QueryDisplayConfig(
            QueryDeviceConfigFlags flags,
            ref uint numPathArrayElements, [Out] DisplayconfigPathInfo[] pathInfoArray,
            ref uint numModeInfoArrayElements, [Out] DisplayconfigModeInfo[] modeInfoArray,
            IntPtr currentTopologyId
        );

        [DllImport("user32.dll")]
        private static extern int DisplayConfigGetDeviceInfo(ref DisplayconfigTargetDeviceName deviceName);

        #endregion
    }
}

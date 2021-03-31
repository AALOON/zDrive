using System;
using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_device_info_header
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigDeviceInfoHeader
    {
        [MarshalAs(UnmanagedType.U4)]
        public readonly DisplayConfigDeviceInfoType Type;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint Size;

        [MarshalAs(UnmanagedType.Struct)]
        public readonly Luid AdapterId;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint Id;

        public DisplayConfigDeviceInfoHeader(Luid adapterId, Type requestType) : this()
        {
            this.AdapterId = adapterId;
            this.Size = (uint)Marshal.SizeOf(requestType);

            if (requestType == typeof(DisplayConfigSourceDeviceName))
            {
                this.Type = DisplayConfigDeviceInfoType.GetSourceName;
            }
            else if (requestType == typeof(DisplayConfigTargetDeviceName))
            {
                this.Type = DisplayConfigDeviceInfoType.GetTargetName;
            }
            else if (requestType == typeof(DisplayConfigTargetPreferredMode))
            {
                this.Type = DisplayConfigDeviceInfoType.GetTargetPreferredMode;
            }
            else if (requestType == typeof(DisplayConfigAdapterName))
            {
                this.Type = DisplayConfigDeviceInfoType.GetAdapterName;
            }
            else if (requestType == typeof(DisplayConfigSetTargetPersistence))
            {
                this.Type = DisplayConfigDeviceInfoType.SetTargetPersistence;
            }
            else if (requestType == typeof(DisplayConfigTargetBaseType))
            {
                this.Type = DisplayConfigDeviceInfoType.GetTargetBaseType;
            }
            else if (requestType == typeof(DisplayConfigGetSourceDPIScale))
            {
                this.Type = DisplayConfigDeviceInfoType.GetSourceDpiScale;
            }
            else if (requestType == typeof(DisplayConfigSetSourceDPIScale))
            {
                this.Type = DisplayConfigDeviceInfoType.SetSourceDpiScale;
            }
            else if (requestType == typeof(DisplayConfigSupportVirtualResolution))
            {
            }
        }

        public DisplayConfigDeviceInfoHeader(Luid adapterId, uint id, Type requestType) :
            this(adapterId, requestType) => this.Id = id;

        public DisplayConfigDeviceInfoHeader(
            Luid adapterId,
            uint id,
            Type requestType,
            DisplayConfigDeviceInfoType request)
            : this(adapterId, id, requestType) =>
            this.Type = request;
    }
}

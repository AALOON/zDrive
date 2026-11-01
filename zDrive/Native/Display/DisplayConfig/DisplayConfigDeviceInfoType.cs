namespace zDrive.Native.Display.DisplayConfig
{
    internal enum DisplayConfigDeviceInfoType
    {
        SetSourceDpiScale = -4,
        GetSourceDpiScale = -3,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME
        /// </summary>
        GetSourceName = 1,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME
        /// </summary>
        GetTargetName = 2,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_PREFERRED_MODE
        /// </summary>
        GetTargetPreferredMode = 3,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_GET_ADAPTER_NAME
        /// </summary>
        GetAdapterName = 4,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_SET_TARGET_PERSISTENCE
        /// </summary>
        SetTargetPersistence = 5,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_BASE_TYPE
        /// </summary>
        GetTargetBaseType = 6,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_GET_SUPPORT_VIRTUAL_RESOLUTION
        /// </summary>
        GetSupportVirtualResolution = 7,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_SET_SUPPORT_VIRTUAL_RESOLUTION
        /// </summary>
        SetSupportVirtualResolution = 8,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_GET_ADVANCED_COLOR_INFO
        /// </summary>
        DeviceInfoGetAdvancedColorInfo = 9,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_SET_ADVANCED_COLOR_STATE
        /// </summary>
        DeviceInfoSetAdvancedColorState = 10,

        /// <summary>
        /// DISPLAYCONFIG_DEVICE_INFO_GET_SDR_WHITE_LEVEL
        /// </summary>
        DeviceInfoGetSdrWhiteLevel = 11,
    }
}

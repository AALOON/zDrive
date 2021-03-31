namespace zDrive.Native.Display.DisplayConfig
{
    internal enum DisplayConfigDeviceInfoType
    {
        SetSourceDpiScale = -4,
        GetSourceDpiScale = -3,
        GetSourceName = 1,
        GetTargetName = 2,
        GetTargetPreferredMode = 3,
        GetAdapterName = 4,
        SetTargetPersistence = 5,
        GetTargetBaseType = 6,
        GetSupportVirtualResolution = 7,
        SetSupportVirtualResolution = 8
    }
}

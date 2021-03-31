namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// iModeNum
    /// The type of information to be retrieved. This value can be a graphics mode index or one of the following values.
    /// </summary>
    internal enum DisplaySettingsMode
    {
        /// <summary>
        /// ENUM_CURRENT_SETTINGS
        /// Retrieve the current settings for the display device.
        /// </summary>
        CurrentSettings = -1,

        /// <summary>
        /// Retrieve the settings for the display device that are currently stored in the registry.
        /// </summary>
        RegistrySettings = -2
    }
}

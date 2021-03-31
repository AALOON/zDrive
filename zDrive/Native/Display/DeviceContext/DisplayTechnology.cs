namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/nf-wingdi-getdevicecaps
    /// TECHNOLOGY
    /// Device technology. It can be any one of the following values.
    /// </summary>
    internal enum DisplayTechnology
    {
        /// <summary>
        /// Vector plotter
        /// </summary>
        Plotter = 0,

        /// <summary>
        /// Raster display
        /// </summary>
        RasterDisplay = 1,

        /// <summary>
        /// Raster printer
        /// </summary>
        RasterPrinter = 2,

        /// <summary>
        /// Raster camera
        /// </summary>
        RasterCamera = 3,

        /// <summary>
        /// Character stream
        /// </summary>
        CharacterStream = 4,

        /// <summary>
        /// Metafile
        /// </summary>
        MetaFile = 5,

        /// <summary>
        /// Display file
        /// </summary>
        DisplayFile = 6
    }
}

namespace zDrive.Native.Display.DeviceContext
{
    /// <summary>
    /// https://github.com/lxn/win/blob/master/gdi32.go
    /// </summary>
    internal enum DeviceCapability
    {
        DriverVersion = 0,
        Technology = 2,
        HorizontalSizeInMm = 4,
        VerticalSizeInMm = 6,
        HorizontalResolution = 8,
        VerticalResolution = 10,
        BitsPerPixel = 12,
        Planes = 14,
        NumberOfBrushes = 16,
        NumberOfPens = 18,
        NumberOfMarkers = 20,
        NumberOfFonts = 22,
        NumberOfColors = 24,
        DeviceDescriptorSize = 26,
        CurveCapabilities = 28,
        LineCapabilities = 30,
        PolygonalCapabilities = 32,
        TextCapabilities = 34,
        ClipCapabilities = 36,
        RasterCapabilities = 38,
        HorizontalAspect = 40,
        VerticalAspect = 42,
        HypotenuseAspect = 44,

        ShadeBlendingCapabilities = 45,
        HorizontalLogicalPixels = 88,
        VerticalLogicalPixels = 90,
        PaletteSize = 104,
        ReservedPaletteSize = 106,
        ColorResolution = 108,

        // Printer
        PhysicalWidth = 110,
        PhysicalHeight = 111,
        PhysicalHorizontalMargin = 112,
        PhysicalVerticalMargin = 113,
        HorizontalScalingFactor = 114,
        VerticalScalingFactor = 115,

        // Display
        VerticalRefreshRateInHz = 116,
        DesktopVerticalResolution = 117,
        DesktopHorizontalResolution = 118,
        PreferredBitAlignment = 119,
        DisplayShadeBlendingCapabilities = 120,
        ColorManagementCapabilities = 121
    }
}

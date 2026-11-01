using System;
using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows-hardware/drivers/display/connecting-and-configuring-displays
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ne-wingdi-displayconfig_device_info_type
    /// http://forum.doom9.net/showthread.php?t=177375&page=2
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigGetAdvancedColorInfo : IEquatable<DisplayConfigGetAdvancedColorInfo>
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigDeviceInfoHeader Header;

        public readonly uint ColorMask;
        public readonly DisplayConfigColorEncoding ColorEncoding;
        public readonly int BitsPerColorChannel;

        public bool IsAdvancedColorSupported => (this.ColorMask & 0x1) == 0x1;

        public bool IsAdvancedColorEnabled => (this.ColorMask & 0x2) == 0x2;

        public bool IsWideColorEnforced => (this.ColorMask & 0x4) == 0x4;

        public bool IsAdvancedColorForceDisabled => (this.ColorMask & 0x8) == 0x8;

        public DisplayConfigGetAdvancedColorInfo(Luid adapter, uint targetId) : this() =>
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, targetId, this.GetType(),
                DisplayConfigDeviceInfoType.DeviceInfoGetAdvancedColorInfo);

        /// <inheritdoc />
        public bool Equals(DisplayConfigGetAdvancedColorInfo other) => this.Header.Equals(other.Header) &&
                                                                       this.ColorMask == other.ColorMask &&
                                                                       this.ColorEncoding == other.ColorEncoding &&
                                                                       this.BitsPerColorChannel ==
                                                                       other.BitsPerColorChannel;

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is DisplayConfigGetAdvancedColorInfo other && this.Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(this.Header, this.ColorMask, (int)this.ColorEncoding,
            this.BitsPerColorChannel);
    }
}

using System;
using System.Runtime.InteropServices;
using zDrive.Native.Display.Structures;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_set_advanced_color_state
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigSetAdvancedColorInfo : IEquatable<DisplayConfigSetAdvancedColorInfo>
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        [MarshalAs(UnmanagedType.Struct)]
        public readonly DisplayConfigDeviceInfoHeader Header;

        public readonly uint StateMask;

        public bool IsAdvancedColorEnabled => (this.StateMask & 0x1) == 0x1;

        public DisplayConfigSetAdvancedColorInfo(Luid adapter, uint targetId, bool enableAdvancedColor) : this()
        {
            this.StateMask = enableAdvancedColor ? 1u : 0u;
            this.Header = new DisplayConfigDeviceInfoHeader(adapter, targetId, this.GetType(),
                DisplayConfigDeviceInfoType.DeviceInfoSetAdvancedColorState);
        }

        /// <inheritdoc />
        public bool Equals(DisplayConfigSetAdvancedColorInfo other) => this.Header.Equals(other.Header) &&
                                                                       this.StateMask == other.StateMask;

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            obj is DisplayConfigSetAdvancedColorInfo other && this.Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(this.Header, this.StateMask);
    }
}

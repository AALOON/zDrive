using System;
using System.Runtime.InteropServices;

namespace zDrive.Native.Display.Structures
{
    /// <summary>
    /// The Luid structure is 64-bit value guaranteed to be unique only on the system on which it was generated. The uniqueness
    /// of a locally unique identifier (Luid) is guaranteed only until the system is restarted.
    /// https://docs.microsoft.com/en-us/windows/win32/api/winnt/ns-winnt-luid
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Luid : IEquatable<Luid>
    {
        /// <summary>
        /// Specifies a DWORD that contains the unsigned lower numbers of the id.
        /// </summary>
        public readonly uint LowPart;

        /// <summary>
        /// Specifies a LONG that contains the signed high numbers of the id.
        /// </summary>
        public readonly int HighPart;

        public Luid(uint lowPart, int highPart)
        {
            this.LowPart = lowPart;
            this.HighPart = highPart;
        }

        /// <inheritdoc />
        public bool Equals(Luid other) => this.LowPart == other.LowPart && this.HighPart == other.HighPart;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is Luid luid && this.Equals(luid);
        }

        public static bool operator ==(Luid left, Luid right) => left.Equals(right);

        public static bool operator !=(Luid left, Luid right) => !(left == right);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(this.LowPart, this.HighPart);

        /// <summary>
        /// Checks if this type is empty and holds no real data
        /// </summary>
        /// <returns>true if empty, otherwise false</returns>
        public bool IsEmpty() => this.LowPart == 0 && this.HighPart == 0;

        /// <summary>
        /// Returns an empty instance of this type
        /// </summary>
        public static Luid Empty => default;

        /// <inheritdoc />
        public override string ToString() => $"{this.LowPart:X}-{this.HighPart:X}";
    }
}

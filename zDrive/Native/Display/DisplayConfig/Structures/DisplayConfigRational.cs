using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace zDrive.Native.Display.DisplayConfig.Structures
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-displayconfig_rational
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DisplayConfigRational : IEquatable<DisplayConfigRational>
    {
        [MarshalAs(UnmanagedType.U4)]
        public readonly uint Numerator;

        [MarshalAs(UnmanagedType.U4)]
        public readonly uint Denominator;

        public DisplayConfigRational(uint numerator, uint denominator, bool simplify)
            : this((ulong)numerator, denominator, simplify)
        {
        }

        public DisplayConfigRational(ulong numerator, ulong denominator, bool simplify)
        {
            var gcm = simplify & (numerator != 0) ? Euclidean(numerator, denominator) : 1;
            this.Numerator = (uint)(numerator / gcm);
            this.Denominator = (uint)(denominator / gcm);
        }

        private static ulong Euclidean(ulong a, ulong b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return a == 0 ? b : a;
        }

        [Pure]
        public ulong ToValue(ulong multiplier = 1)
        {
            if (this.Numerator == 0)
            {
                return 0;
            }

            return this.Numerator * multiplier / this.Denominator;
        }

        public bool Equals(DisplayConfigRational other)
        {
            if (this.Numerator == other.Numerator && this.Denominator == other.Denominator)
            {
                return true;
            }

            var left = this.Numerator / (double)this.Denominator;
            var right = other.Numerator / (double)other.Denominator;

            return Math.Abs(left - right) <= Math.Max(Math.Abs(left), Math.Abs(right)) * 1E-15;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is DisplayConfigRational rational && this.Equals(rational);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)this.Numerator * 397) ^ (int)this.Denominator;
            }
        }

        public static bool operator ==(DisplayConfigRational left, DisplayConfigRational right) =>
            Equals(left, right) || left.Equals(right);

        public static bool operator !=(DisplayConfigRational left, DisplayConfigRational right) => !(left == right);
    }
}

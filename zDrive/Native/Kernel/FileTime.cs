using System.Runtime.InteropServices;

namespace zDrive.Native.Kernel
{
    /// <summary>
    /// Contains a 64-bit value representing the number of 100-nanosecond intervals since January 1, 1601 (UTC).
    /// FILETIME
    /// https://docs.microsoft.com/en-us/windows/win32/api/minwinbase/ns-minwinbase-filetime
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct FileTime
    {
        public uint LowDateTime;
        public uint HighDateTime;

        public ulong ToLong() => ((ulong)this.HighDateTime << 32) + this.LowDateTime;
    }
}

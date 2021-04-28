using System.Runtime.InteropServices;

namespace zDrive.Native.Kernel
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/
    /// </summary>
    internal static class Kernel32Api
    {
        private const string Kernel32 = "kernel32.dll";

        /// <summary>
        /// Retrieves system timing information. On a multiprocessor system, the values returned are the sum of the designated
        /// times across all processors.
        /// https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getsystemtimes
        /// </summary>
        [DllImport(Kernel32, SetLastError = true)]
        public static extern bool GetSystemTimes(out FileTime idleTime, out FileTime kernelTime,
            out FileTime userTime);
    }
}

using System.Runtime.InteropServices;

namespace zDrive.Native.Ps
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/psapi/psapi-functions
    /// </summary>
    internal static class PsApi
    {
        private const string PsApiDll = "psapi.dll";

        /// <summary>
        /// Retrieves the performance values contained in the PERFORMANCE_INFORMATION structure.
        /// https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getperformanceinfo
        /// </summary>
        [DllImport(PsApiDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo([Out] out PerformanceInformation performanceInformation,
            [In] int size);
    }
}

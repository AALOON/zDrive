using System;
using System.Runtime.InteropServices;

namespace zDrive.Native.Pdh
{
    internal static class PdhApi
    {
        /// <summary>
        /// The requested format for the  API.
        /// https://docs.microsoft.com/en-us/windows/win32/api/pdh/nf-pdh-pdhgetformattedcountervalue
        /// </summary>
        public const uint PdhFmtDouble = 0x00000200;

        private const string PdhDll = "pdh.dll";

        /// <summary>
        /// Adds counter to query
        /// </summary>
        [DllImport(PdhDll, CharSet = CharSet.Unicode)]
        public static extern PdhErrorCode PdhAddCounter(PdhQueryHandle hQuery, string counterPath, IntPtr userData,
            out PdhCounterHandle counter);

        /// <summary>
        /// Closes Query object handle
        /// </summary>
        [DllImport(PdhDll)]
        public static extern PdhErrorCode PdhCloseQuery(IntPtr hQuery);

        /// <summary>
        /// Retrieves a sample from the source
        /// </summary>
        [DllImport(PdhDll)]
        public static extern PdhErrorCode PdhCollectQueryData(PdhQueryHandle phQuery);

        /// <summary>
        /// Retrieves a specific counter value in the specified format.
        /// </summary>
        [DllImport(PdhDll)]
        public static extern PdhErrorCode PdhGetFormattedCounterValue(
            PdhCounterHandle phCounter,
            uint dwFormat,
            IntPtr lpdwType,
            out PdhFormattedCounterValue pValue);

        /// <summary>
        /// Creates Query object
        /// </summary>
        [DllImport(PdhDll, CharSet = CharSet.Unicode)]
        public static extern PdhErrorCode PdhOpenQuery(string source, IntPtr userData,
            out PdhQueryHandle queryHandle);

        /// <summary>
        /// Removes counter from query
        /// </summary>
        [DllImport(PdhDll)]
        public static extern PdhErrorCode PdhRemoveCounter(IntPtr hCounter);
    }
}

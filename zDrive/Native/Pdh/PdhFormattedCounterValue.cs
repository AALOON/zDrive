using System.Runtime.InteropServices;

namespace zDrive.Native.Pdh
{
    /// <summary>
    /// The value of a counter as returned by  API.
    /// It corresponds to original PDH_FMT_COUNTERVALUE.
    /// https://docs.microsoft.com/en-us/windows/win32/api/pdh/ns-pdh-pdh_fmt_countervalue
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PdhFormattedCounterValue
    {
        public PdhErrorCode CStatus;
        public double DoubleValue;
    }
}

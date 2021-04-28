namespace zDrive.Native.Pdh
{
    /// <summary>
    /// Return codes and statuses of pdh functions.
    /// https://docs.microsoft.com/en-us/windows/win32/perfctrs/pdh-error-codes
    /// </summary>
    public enum PdhErrorCode : uint
    {
        /// <summary>
        /// ERROR_SUCCESS, PDH_CSTATUS_VALID_DATA
        /// </summary>
        Success = 0,

        /// <summary>
        /// PDH_NO_MORE_DATA
        /// </summary>
        NoMoreData = 0xC0000BCC,

        /// <summary>
        /// PDH_INVALID_DATA
        /// </summary>
        InvalidData = 0xC0000BC6
    }
}

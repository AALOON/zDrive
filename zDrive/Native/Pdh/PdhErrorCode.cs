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
        /// PDH_CSTATUS_NO_MACHINE
        /// </summary>
        NoMachine = 0xC0000BC0,

        /// <summary>
        /// PDH_CSTATUS_NO_OBJECT
        /// </summary>
        NoObject = 0xC0000BC1,

        /// <summary>
        /// PDH_CSTATUS_NO_COUNTER
        /// </summary>
        NoCounter = 0xC0000BC2,

        /// <summary>
        /// PDH_INVALID_HANDLE
        /// </summary>
        InvalidHandle = 0xC0000BC5,

        /// <summary>
        /// PDH_CSTATUS_NO_INSTANCE
        /// </summary>
        NoInstance = 0xC0000BC7,

        /// <summary>
        /// PDH_INVALID_ARGUMENT
        /// </summary>
        InvalidArgument = 0xC0000BC8,

        /// <summary>
        /// PDH_INVALID_PATH
        /// </summary>
        InvalidPath = 0xC0000BC9,

        /// <summary>
        /// PDH_INVALID_DATA
        /// </summary>
        InvalidData = 0xC0000BC6
    }
}

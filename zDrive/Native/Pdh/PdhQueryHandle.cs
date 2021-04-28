using Microsoft.Win32.SafeHandles;

namespace zDrive.Native.Pdh
{
    internal sealed class PdhQueryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// PDH query handle
        /// </summary>
        public PdhQueryHandle()
            : base(true)
        {
        }

        /// <inheritdoc />
        protected override bool ReleaseHandle() => PdhApi.PdhCloseQuery(this.handle) == PdhErrorCode.Success;
    }
}

using Microsoft.Win32.SafeHandles;

namespace zDrive.Native.Pdh
{
    internal sealed class PdhCounterHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public PdhCounterHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle() => PdhApi.PdhRemoveCounter(this.handle) == PdhErrorCode.Success;
    }
}

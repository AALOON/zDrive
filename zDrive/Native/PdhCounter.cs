using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using zDrive.Native.Pdh;
using zDrive.Services.Ioc;

namespace zDrive.Native
{
    public sealed class PdhCounter : IPdhCounter
    {
        private readonly string counterPath;
        private readonly OneTimeSetup cpuCounterInitialized = new();
        private readonly ILogger logger;

        private PdhCounterHandle cpuCounterHandle;
        private PdhQueryHandle cpuCounterQuery;

        public PdhCounter(string counterPath, ILoggerFactory loggerFactory)
        {
            this.counterPath = counterPath;
            this.logger = loggerFactory.CreateLogger<PdhCounter>();
            this.InitializeCounters();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public double? Collect()
        {
            var result = PdhApi.PdhCollectQueryData(this.cpuCounterQuery);
            if (result != PdhErrorCode.Success)
            {
                var error = Marshal.GetLastWin32Error();
                this.logger.LogWarning("Error on PdhCollectQueryData. Error: {Error}", error);
                return null;
            }

            var pdhResult = PdhApi.PdhGetFormattedCounterValue(this.cpuCounterHandle, PdhApi.PdhFmtDouble,
                IntPtr.Zero,
                out var value);
            if (pdhResult != PdhErrorCode.Success)
            {
                var error = Marshal.GetLastWin32Error();
                this.logger.LogWarning("Error on PdhGetFormattedCounterValue. Error: {Error}", error);
                return null;
            }

            return value.DoubleValue;
        }

        /// <inheritdoc />
        public void InitializeCounters()
        {
            if (this.cpuCounterInitialized.IsSet || !this.cpuCounterInitialized.TrySet())
            {
                return;
            }

            PdhApi.PdhOpenQuery(null, IntPtr.Zero, out this.cpuCounterQuery);

            PdhApi.PdhAddCounter(this.cpuCounterQuery, this.counterPath, IntPtr.Zero,
                out this.cpuCounterHandle);
        }

        private void Dispose(bool disposing)
        {
            this.ReleaseUnmanagedResources();
            if (disposing)
            {
                // release disposal
            }
        }

        private void ReleaseUnmanagedResources()
        {
            this.cpuCounterHandle?.Dispose();
            this.cpuCounterQuery?.Dispose();
        }

        /// <inheritdoc />
        ~PdhCounter() => this.Dispose(false);
    }
}

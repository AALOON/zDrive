using System;
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
        private bool failed;
        private PdhErrorCode? lastError;

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
            if (this.failed)
            {
                return null;
            }

            var result = PdhApi.PdhCollectQueryData(this.cpuCounterQuery);
            if (result != PdhErrorCode.Success)
            {
                this.SetError(result, "Error on PdhCollectQueryData.");
                return null;
            }

            var pdhResult = PdhApi.PdhGetFormattedCounterValue(this.cpuCounterHandle, PdhApi.PdhFmtDouble,
                IntPtr.Zero,
                out var value);
            if (pdhResult != PdhErrorCode.Success)
            {
                this.SetError(pdhResult, "Error on PdhGetFormattedCounterValue.");
                return null;
            }

            this.SetError(null, null);
            return value.DoubleValue;
        }

        /// <inheritdoc />
        public void InitializeCounters()
        {
            if (this.cpuCounterInitialized.IsSet || !this.cpuCounterInitialized.TrySet())
            {
                return;
            }

            var openResult = PdhApi.PdhOpenQuery(null, IntPtr.Zero, out this.cpuCounterQuery);
            if (openResult != PdhErrorCode.Success)
            {
                this.MarkFailed("Error on PdhOpenQuery.", openResult);
                return;
            }

            var addResult = PdhApi.PdhAddEnglishCounter(this.cpuCounterQuery, this.counterPath, IntPtr.Zero,
                out this.cpuCounterHandle);
            if (addResult != PdhErrorCode.Success)
            {
                this.MarkFailed("Error on PdhAddEnglishCounter.", addResult);
            }
        }

        private void SetError(PdhErrorCode? error, string message)
        {
            if (this.lastError == error)
            {
                return;
            }

            this.lastError = error;
            if (error != null)
            {
                this.logger.LogWarning("{Message} Counter path: {CounterPath}. Error: {Error}.", message,
                    this.counterPath, error);
            }
        }

        private void MarkFailed(string message, PdhErrorCode error)
        {
            this.failed = true;
            this.lastError = error;
            this.logger.LogWarning(
                "{Message} Counter path: {CounterPath}. Error: {Error}. Counter is disabled.", message,
                this.counterPath, error);
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

using System;
using System.Threading.Tasks;
using zDrive.Interfaces;

namespace zDrive.Tests.Helpers
{
    /// <summary>
    /// Test implementation of <see cref="IErrorHandler" /> that records the handled exception.
    /// </summary>
    public sealed class TestErrorHandler : IErrorHandler
    {
        private readonly TaskCompletionSource<bool> handled = new();

        /// <summary>
        /// Gets the exception passed to <see cref="HandleError" /> or null.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Task that completes when an error is handled.
        /// </summary>
        public Task Handled => this.handled.Task;

        /// <inheritdoc />
        public void HandleError(Exception ex)
        {
            this.Exception = ex;
            this.handled.SetResult(true);
        }
    }
}

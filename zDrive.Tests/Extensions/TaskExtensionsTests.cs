using System;
using System.Threading;
using System.Threading.Tasks;
using zDrive.Extensions;
using zDrive.Tests.Helpers;
using Xunit;

namespace zDrive.Tests.Extensions
{
    public class TaskExtensionsTests
    {
        [Fact]
        public void FireAndForgetSafeAsync_CompletedTask_DoesNotCallHandler()
        {
            var handler = new TestErrorHandler();

            Task.CompletedTask.FireAndForgetSafeAsync(handler);

            Assert.Null(handler.Exception);
        }

        [Fact]
        public async Task FireAndForgetSafeAsync_FaultedTask_CallsHandler()
        {
            var handler = new TestErrorHandler();
            var expected = new InvalidOperationException("boom");
            var source = new TaskCompletionSource();
            source.SetException(expected);

            source.Task.FireAndForgetSafeAsync(handler);

            await handler.Handled;
            Assert.Same(expected, handler.Exception);
        }

        [Fact]
        public async Task FireAndForgetSafeAsync_FaultedTaskWithoutHandler_DoesNotThrow()
        {
            var source = new TaskCompletionSource();
            source.SetException(new InvalidOperationException("boom"));

            source.Task.FireAndForgetSafeAsync();

            // Give the fire-and-forget continuation a chance to run; no unhandled crash means pass.
            await Task.Delay(50);
        }

        [Fact]
        public async Task FireAndForgetSafeAsync_LongRunningTask_CanBeObserved()
        {
            var handler = new TestErrorHandler();
            var source = new TaskCompletionSource();

            source.Task.FireAndForgetSafeAsync(handler);
            Assert.Null(handler.Exception);

            source.SetResult();
            await Task.Delay(50);

            Assert.Null(handler.Exception);
        }
    }
}

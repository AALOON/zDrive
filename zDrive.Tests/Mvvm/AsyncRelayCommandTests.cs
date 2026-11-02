using System;
using System.Threading.Tasks;
using System.Windows.Input;
using zDrive.Mvvm;
using zDrive.Tests.Helpers;
using Xunit;

namespace zDrive.Tests.Mvvm
{
    public class AsyncRelayCommandTests
    {
        [Fact]
        public async Task ExecuteAsync_RunsDelegate()
        {
            var executed = false;
            var command = new AsyncRelayCommand(() =>
            {
                executed = true;
                return Task.CompletedTask;
            });

            await command.ExecuteAsync();

            Assert.True(executed);
        }

        [Fact]
        public async Task ExecuteAsync_WhenCanExecuteFalse_SkipsExecution()
        {
            var executed = false;
            var command = new AsyncRelayCommand(() =>
            {
                executed = true;
                return Task.CompletedTask;
            }, () => false);

            await command.ExecuteAsync();

            Assert.False(executed);
        }

        [Fact]
        public async Task CanExecute_ReturnsFalseWhileExecuting()
        {
            var completion = new TaskCompletionSource();
            var canExecuteWhileRunning = true;
            AsyncRelayCommand command = null!;
            command = new AsyncRelayCommand(() =>
            {
                canExecuteWhileRunning = command.CanExecute();
                return completion.Task;
            });

            var run = command.ExecuteAsync();
            completion.SetResult();
            await run;

            Assert.False(canExecuteWhileRunning);
            Assert.True(command.CanExecute());
        }

        [Fact]
        public void RaiseCanExecuteChanged_RaisesEvent()
        {
            var command = new AsyncRelayCommand(() => Task.CompletedTask);
            var raised = 0;
            command.CanExecuteChanged += (_, _) => raised++;

            command.RaiseCanExecuteChanged();

            Assert.Equal(1, raised);
        }

        [Fact]
        public void ICommand_CanExecute_DelegatesToCanExecute()
        {
            ICommand command = new AsyncRelayCommand(() => Task.CompletedTask, () => false);

            Assert.False(command.CanExecute(null));
        }

        [Fact]
        public async Task ICommand_Execute_FaultingTask_CallsErrorHandler()
        {
            var handler = new TestErrorHandler();
            ICommand command = new AsyncRelayCommand(
                () => Task.FromException(new InvalidOperationException()), null, handler);

            command.Execute(null);

            await handler.Handled;
            Assert.IsType<InvalidOperationException>(handler.Exception);
        }
    }
}

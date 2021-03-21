using System;
using System.Threading.Tasks;
using System.Windows.Input;
using zDrive.Extensions;
using zDrive.Interfaces;

namespace zDrive.Mvvm
{
    public class AsyncRelayCommand : IAsyncCommand
    {
        private readonly Func<bool> canExecute;
        private readonly IErrorHandler errorHandler;
        private readonly Func<Task> execute;

        private bool isExecuting;

        public AsyncRelayCommand(
            Func<Task> execute,
            Func<bool> canExecute = null,
            IErrorHandler errorHandler = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.errorHandler = errorHandler;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute() => !this.isExecuting && (this.canExecute?.Invoke() ?? true);

        public async Task ExecuteAsync()
        {
            if (this.CanExecute())
            {
                try
                {
                    this.isExecuting = true;
                    await this.execute();
                }
                finally
                {
                    this.isExecuting = false;
                }
            }

            this.RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #region Explicit implementations

        bool ICommand.CanExecute(object parameter) => this.CanExecute();

        void ICommand.Execute(object parameter) => this.ExecuteAsync().FireAndForgetSafeAsync(this.errorHandler);

        #endregion
    }
}

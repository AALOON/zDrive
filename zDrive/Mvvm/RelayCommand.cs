using System;
using System.Diagnostics;
using System.Windows.Input;

namespace zDrive.Mvvm
{
    /// <summary>
    /// This RelayCommand is taken from MSDN magazine
    /// http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Constructors

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));

            this.canExecute = canExecute;
        }

        #endregion Constructors

        public static RelayCommand Empty { get; } = new(_ => { });

        #region Fields

        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        #endregion Fields

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter) => this.canExecute?.Invoke(parameter) ?? true;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter) => this.execute(parameter);

        #endregion // ICommand Members
    }
}

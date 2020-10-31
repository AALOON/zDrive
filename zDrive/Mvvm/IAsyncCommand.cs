using System.Threading.Tasks;
using System.Windows.Input;

namespace zDrive.Mvvm
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }
}
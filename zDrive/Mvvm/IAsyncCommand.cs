using System.Threading.Tasks;
using System.Windows.Input;

namespace zDrive.Mvvm
{
    public interface IAsyncCommand : ICommand
    {
        bool CanExecute();

        Task ExecuteAsync();
    }
}

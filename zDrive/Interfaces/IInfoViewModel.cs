using zDrive.Helpers;

namespace zDrive.Interfaces
{
    public interface IInfoViewModel
    {
        RelayCommand OpenCommand { get; }

        string Key { get; }

        string Name { get; }

        string Info { get; }

        double Value { get; }

        void RaiseChanges();
    }
}
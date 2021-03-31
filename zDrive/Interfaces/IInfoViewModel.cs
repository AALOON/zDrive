using zDrive.Mvvm;

namespace zDrive.Interfaces
{
    public interface IInfoViewModel
    {
        RelayCommand LeftMouseCommand { get; }

        RelayCommand RightMouseCommand { get; }

        string Key { get; }

        string Name { get; }

        string DisplayString { get; }

        string Info { get; }

        double Value { get; }

        void RaiseChanges();
    }
}

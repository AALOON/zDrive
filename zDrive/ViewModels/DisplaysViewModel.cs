using System;
using System.Diagnostics;
using System.Threading.Tasks;
using zDrive.Collections;
using zDrive.Interfaces;
using zDrive.Mvvm;
using zDrive.Native;

namespace zDrive.ViewModels
{
    /// <summary>
    ///     VM for displays
    /// </summary>
    internal sealed class DisplaysViewModel : ViewModelBase, IInfoViewModel
    {
        private readonly IInfoFormatter _format;

        internal DisplaysViewModel(IInfoFormatter format)
        {
            _format = format;

            ReloadMonitor();
        }

        public ExtendedObservableCollection<DisplayViewModel> Displays { get; set; } =
            new ExtendedObservableCollection<DisplayViewModel>();

        public RelayCommand OpenCommand { get; } = null;

        public string Key => "Displays";
        public string Name => "Displays";
        public string DisplayString => "Displays";
        public string Info { get; }
        public double Value => 0;

        public void RaiseChanges()
        {
            RaisePropertyChanged(nameof(Info));
            RaisePropertyChanged(nameof(Value));
            RaisePropertyChanged(nameof(Displays));
        }

        private void ReloadMonitor()
        {
            Displays.SuppressNotification = true;
            Displays.Clear();
            foreach (var screen in MonitorsService.AllMonitorDevices())
                Displays.Add(new DisplayViewModel(ReloadMonitor)
                {
                    DeviceName = screen.DeviceName.Replace(@"\\.\", ""),
                    FriendlyName = screen.FriendlyName,
                    IsPrimary = screen.IsPrimary,
                    DisplayId = screen.Index
                });

            Displays.SuppressNotification = false;
            Displays.RaiseChanged();
            RaiseChanges();
        }
    }

    /// <summary>
    ///     VM for one display.
    /// </summary>
    public sealed class DisplayViewModel : ViewModelBase
    {
        private readonly Action _updateDisplays;

        public DisplayViewModel(Action updateDisplays)
        {
            _updateDisplays = updateDisplays;
            SelectCommand = new AsyncRelayCommand(SelectMonitorAsync);
        }

        public string DeviceName { get; set; }

        public string FriendlyName { get; set; }

        public bool IsPrimary { get; set; }

        public uint DisplayId { get; set; }

        public AsyncRelayCommand SelectCommand { get; }

        public void RaiseChanges()
        {
            RaisePropertyChanged(nameof(IsPrimary));
            RaisePropertyChanged(nameof(DeviceName));
            RaisePropertyChanged(nameof(FriendlyName));
        }

        private async Task SelectMonitorAsync()
        {
            MonitorChanger.SetAsPrimaryMonitor(DisplayId);
            var sw = Stopwatch.StartNew();
            while (!MonitorChanger.IsPrime(DisplayId) && sw.Elapsed < TimeSpan.FromSeconds(5)) await Task.Delay(50);

            _updateDisplays();
        }
    }
}
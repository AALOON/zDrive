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
    /// VM for displays
    /// </summary>
    internal sealed class DisplaysViewModel : ViewModelBase, IInfoViewModel
    {
        private readonly IInfoFormatter format;

        internal DisplaysViewModel(IInfoFormatter format)
        {
            this.format = format;

            this.ReloadMonitor();
        }

        public ExtendedObservableCollection<DisplayViewModel> Displays { get; set; } =
            new();

        public RelayCommand OpenCommand { get; } = null;

        public string Key => "Displays";

        public string Name => "Displays";

        public string DisplayString => "Displays";

        public string Info { get; }

        public double Value => 0;

        public void RaiseChanges()
        {
            this.RaisePropertyChanged(nameof(this.Info));
            this.RaisePropertyChanged(nameof(this.Value));
            this.RaisePropertyChanged(nameof(this.Displays));
        }

        private void ReloadMonitor()
        {
            this.Displays.SuppressNotification = true;
            this.Displays.Clear();
            foreach (var screen in MonitorsService.AllMonitorDevices())
            {
                this.Displays.Add(new DisplayViewModel(this.ReloadMonitor)
                {
                    DeviceName = screen.DeviceName.Replace(@"\\.\", ""),
                    FriendlyName = screen.FriendlyName,
                    IsPrimary = screen.IsPrimary,
                    DisplayId = screen.Index
                });
            }

            this.Displays.SuppressNotification = false;
            this.Displays.RaiseChanged();
            this.RaiseChanges();
        }
    }

    /// <summary>
    /// VM for one display.
    /// </summary>
    public sealed class DisplayViewModel : ViewModelBase
    {
        private readonly Action updateDisplays;

        public DisplayViewModel(Action updateDisplays)
        {
            this.updateDisplays = updateDisplays;
            this.SelectCommand = new AsyncRelayCommand(this.SelectMonitorAsync);
        }

        public string DeviceName { get; set; }

        public string FriendlyName { get; set; }

        public bool IsPrimary { get; set; }

        public uint DisplayId { get; set; }

        public AsyncRelayCommand SelectCommand { get; }

        public void RaiseChanges()
        {
            this.RaisePropertyChanged(nameof(this.IsPrimary));
            this.RaisePropertyChanged(nameof(this.DeviceName));
            this.RaisePropertyChanged(nameof(this.FriendlyName));
        }

        private async Task SelectMonitorAsync()
        {
            MonitorChanger.SetAsPrimaryMonitor(this.DisplayId);
            var sw = Stopwatch.StartNew();
            while (!MonitorChanger.IsPrime(this.DisplayId) && sw.Elapsed < TimeSpan.FromSeconds(5))
            {
                await Task.Delay(50);
            }

            this.updateDisplays();
        }
    }
}

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
        private bool showHdrButton;

        internal DisplaysViewModel() => this.ReloadMonitor();

        public ExtendedObservableCollection<DisplayViewModel> Displays { get; set; } = new();

        /// <summary>
        /// Shows hdr buttons on displays. Is set from the settings.
        /// </summary>
        public bool ShowHdrButton
        {
            get => this.showHdrButton;
            set
            {
                if (this.Set(ref this.showHdrButton, value))
                {
                    foreach (var display in this.Displays)
                    {
                        display.RaiseHdrChanges();
                    }
                }
            }
        }

        public RelayCommand LeftMouseCommand { get; } = RelayCommand.Empty;

        /// <inheritdoc />
        public RelayCommand RightMouseCommand { get; } = RelayCommand.Empty;

        public string Key => "Displays";

        public string Name => "Displays";

        public string DisplayString => "Displays";

        public string Info { get; }

        public double Value => 0;

        public void RaiseChanges()
        {
            this.UpdateHdrStates();
            this.RaisePropertyChanged(nameof(this.Info));
            this.RaisePropertyChanged(nameof(this.Value));
            this.RaisePropertyChanged(nameof(this.Displays));
        }

        private void UpdateHdrStates()
        {
            foreach (var display in this.Displays)
            {
                try
                {
                    display.UpdateHdrState();
                }
                catch (Exception)
                {
                    // Keep the last known hdr state on transient win api errors.
                }

                display.RaiseHdrChanges();
            }
        }

        private void ReloadMonitor()
        {
            this.Displays.SuppressNotification = true;
            this.Displays.Clear();
            foreach (var screen in MonitorsService.Instance.AllMonitorDevices())
            {
                this.Displays.Add(new DisplayViewModel(this.ReloadMonitor, () => this.ShowHdrButton)
                {
                    DeviceName = screen.DeviceName.Replace(@"\\.\", ""),
                    FriendlyName = screen.FriendlyName,
                    IsPrimary = screen.IsPrimary,
                    DisplayId = screen.Index,
                    IsHdrSupported = screen.IsHdrSupported,
                    IsHdrEnabled = screen.IsHdrEnabled
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
        private readonly Func<bool> isHdrButtonVisible;
        private readonly Action updateDisplays;

        public DisplayViewModel(Action updateDisplays, Func<bool> isHdrButtonVisible)
        {
            this.updateDisplays = updateDisplays;
            this.isHdrButtonVisible = isHdrButtonVisible;
            this.SelectCommand = new AsyncRelayCommand(this.SelectMonitorAsync);
            this.ToggleHdrCommand = new AsyncRelayCommand(this.ToggleHdrAsync);
        }

        public string DeviceName { get; set; }

        public string FriendlyName { get; set; }

        public bool IsPrimary { get; set; }

        public uint DisplayId { get; set; }

        public bool IsHdrSupported { get; set; }

        public bool IsHdrEnabled { get; set; }

        /// <summary>
        /// Shows hdr button on this display. Depends on the settings and hdr support of the display.
        /// </summary>
        public bool ShowHdr => this.isHdrButtonVisible() && this.IsHdrSupported;

        public AsyncRelayCommand SelectCommand { get; }

        public AsyncRelayCommand ToggleHdrCommand { get; }

        public void RaiseChanges()
        {
            this.RaisePropertyChanged(nameof(this.IsPrimary));
            this.RaisePropertyChanged(nameof(this.DeviceName));
            this.RaisePropertyChanged(nameof(this.FriendlyName));
        }

        public void RaiseHdrChanges()
        {
            this.RaisePropertyChanged(nameof(this.IsHdrSupported));
            this.RaisePropertyChanged(nameof(this.IsHdrEnabled));
            this.RaisePropertyChanged(nameof(this.ShowHdr));
        }

        internal void UpdateHdrState()
        {
            var state = MonitorsService.Instance.GetHdrState(this.DeviceName);
            this.IsHdrSupported = state.IsSupported;
            this.IsHdrEnabled = state.IsEnabled;
        }

        private async Task SelectMonitorAsync()
        {
            MonitorsService.Instance.SetAsPrimaryMonitor(this.DisplayId);
            var sw = Stopwatch.StartNew();
            while (!MonitorsService.Instance.IsPrime(this.DisplayId) && sw.Elapsed < TimeSpan.FromSeconds(5))
            {
                await Task.Delay(50);
            }

            this.updateDisplays();
        }

        private async Task ToggleHdrAsync()
        {
            var enable = !this.IsHdrEnabled;
            MonitorsService.Instance.SetHdr(this.DeviceName, enable);
            var sw = Stopwatch.StartNew();
            while (MonitorsService.Instance.IsHdrEnabled(this.DeviceName) != enable &&
                   sw.Elapsed < TimeSpan.FromSeconds(5))
            {
                await Task.Delay(50);
            }

            this.UpdateHdrState();
            this.RaiseHdrChanges();
        }
    }
}

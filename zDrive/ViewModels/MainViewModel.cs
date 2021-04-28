using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using zDrive.Converters;
using zDrive.Interfaces;
using zDrive.Mvvm;
using Application = System.Windows.Application;

namespace zDrive.ViewModels
{
    /// <summary>
    /// VM for main windows.
    /// </summary>
    internal sealed class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IDriveInfoService driveInfoService;
        private readonly IRegistryService registryService;
        private readonly IWidgetsService widgetsService;
        private bool cpu;
        private bool ram;

        private bool showUnavailable;
        private Theme theme;
        private bool topmost;
        private double x, y;

        public MainViewModel(IRegistryService registryService,
            IDriveInfoService driveInfoService,
            IDriveDetectionService detectionService,
            IWidgetsService widgetsService,
            ITimerService timerService,
            IDictionary<string, IDriveViewModel> drives,
            IDictionary<string, IInfoViewModel> widgets,
            ILogger logger)
        {
            this.registryService = registryService;
            this.driveInfoService = driveInfoService;
            this.widgetsService = widgetsService;
            this.Drives = drives;
            this.Widgets = widgets;

            detectionService.DeviceAdded += this.DeviceAdded;
            detectionService.DeviceRemoved += this.DeviceRemoved;

            timerService.Tick += this.TimerTick;

            this.InitRelay();

            this.Initialize();

            this.InitializeWidgets();

            this.CheckIfVersionChanged();
            logger.LogInformation(nameof(zDrive) + " is started.");
        }

        /// <inheritdoc />
        public IDictionary<string, IInfoViewModel> Widgets { get; }

        /// <inheritdoc />
        public IDictionary<string, IDriveViewModel> Drives { get; }

        /// <inheritdoc />
        public bool ShowUnavailable
        {
            get => this.showUnavailable;
            set
            {
                if (this.Set(ref this.showUnavailable, value))
                {
                    this.driveInfoService.ShowUnavailable = value;
                    this.registryService.Write(nameof(this.ShowUnavailable), value);
                }
            }
        }

        /// <inheritdoc />
        public bool Topmost
        {
            get => this.topmost;
            set
            {
                if (this.Set(ref this.topmost, value))
                {
                    this.registryService.Write(nameof(this.Topmost), value);
                }
            }
        }

        /// <inheritdoc />
        public bool Ram
        {
            get => this.ram;
            set
            {
                if (value)
                {
                    this.widgetsService.Add(InfoWidget.RamDisk);
                }
                else
                {
                    this.widgetsService.Remove(InfoWidget.RamDisk);
                }

                if (this.Set(ref this.ram, value))
                {
                    this.registryService.Write(nameof(this.Ram), value);
                }
            }
        }

        /// <inheritdoc />
        public bool Cpu
        {
            get => this.cpu;
            set
            {
                if (value)
                {
                    this.widgetsService.Add(InfoWidget.Cpu);
                }
                else
                {
                    this.widgetsService.Remove(InfoWidget.Cpu);
                }

                if (this.Set(ref this.cpu, value))
                {
                    this.registryService.Write(nameof(this.Cpu), value);
                }
            }
        }

        /// <inheritdoc />
        public double X
        {
            get => this.x;
            set
            {
                if (this.Set(ref this.x, value))
                {
                    this.registryService.Write(nameof(this.X), value);
                }
            }
        }

        /// <inheritdoc />
        public double Y
        {
            get => this.y;
            set
            {
                if (this.Set(ref this.y, value))
                {
                    this.registryService.Write(nameof(this.Y), value);
                }
            }
        }

        /// <inheritdoc />
        public bool AutoRun
        {
            get => this.registryService.ReadAutoRun() != null;
            set
            {
                if (value)
                {
                    var location = GenerateLocation();
                    this.registryService.WriteAutoRun(location);
                }
                else
                {
                    this.registryService.RemoveAutoRun();
                }

                this.RaisePropertyChanged(nameof(this.AutoRun));
            }
        }

        /// <inheritdoc />
        public InfoFormat InfoFormat
        {
            get => this.driveInfoService.InfoFormat;
            set
            {
                this.driveInfoService.InfoFormat = value;

                this.registryService.Write(nameof(this.InfoFormat), (int)value);
                this.RaisePropertyChanged(nameof(this.InfoFormat));
            }
        }

        /// <inheritdoc />
        public Theme Theme
        {
            get => this.theme;
            set
            {
                if (this.theme == value)
                {
                    return;
                }

                var app = (App)Application.Current;
                var nonDefault = value;
                if (nonDefault == Theme.Default)
                {
                    nonDefault = Theme.Gray;
                }

                app.ChangeSkin(new Uri($"/Skins/{nonDefault}Skin.xaml", UriKind.RelativeOrAbsolute));
                this.theme = value;
                this.registryService.Write(nameof(this.Theme), (int)value);
                this.RaisePropertyChanged(nameof(this.Theme));
            }
        }

        private void CheckIfVersionChanged()
        {
            var existingLocation = this.registryService.ReadAutoRun();
            var location = GenerateLocation();
            if (!string.Equals(existingLocation, location, StringComparison.OrdinalIgnoreCase))
            {
                this.registryService.WriteAutoRun(location);
            }
        }

        private void DeviceAdded(object sender, DeviceArrivalEventArgs e) =>
            this.driveInfoService.UpdateAddition(e.Volume);

        private void DeviceRemoved(object sender, DeviceRemovalEventArgs e) =>
            this.driveInfoService.UpdateRemoval(e.Volume);

        private static string GenerateLocation()
        {
            const string dll = ".dll";
            const string exe = ".exe";

            var location = Assembly.GetExecutingAssembly().Location;
            if (location.EndsWith(dll, StringComparison.OrdinalIgnoreCase))
            {
                location = location.Substring(0, location.Length - dll.Length) + exe;
            }

            return location;
        }

        private void Initialize()
        {
            // initialize settings from registry
            this.topmost = this.registryService.Read(nameof(this.Topmost), false);
            this.showUnavailable = this.registryService.Read(nameof(this.ShowUnavailable), false);
            this.InfoFormat = this.registryService.Read(nameof(this.InfoFormat), InfoFormat.Free);
            this.Theme = this.registryService.Read(nameof(this.Theme), Theme.Default);

            this.X = this.registryService.Read(nameof(this.X), 0D);
            this.Y = this.registryService.Read(nameof(this.Y), 0D);

            if (Math.Abs(this.X) < 0.1)
            {
                this.X = SystemParameters.WorkArea.Width - 180;
            }

            if (Math.Abs(this.Y) < 0.1)
            {
                this.Y = 0D;
            }

            try
            {
                if (this.AutoRun) // reinitialize location in case of update.
                {
                    this.AutoRun = true;
                }
            }
            catch (Exception)
            {
                //TODO: write log
            }
        }

        private void InitializeWidgets()
        {
            this.ram = this.registryService.Read(nameof(this.Ram), true);
            if (this.ram)
            {
                this.widgetsService.Add(InfoWidget.RamDisk);
            }

            this.cpu = this.registryService.Read(nameof(this.Cpu), true);
            if (this.cpu)
            {
                this.widgetsService.Add(InfoWidget.Cpu);
            }

            if (Screen.AllScreens.Length > 1)
            {
                this.widgetsService.Add(InfoWidget.Displays);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            foreach (var infoViewModel in this.Widgets)
            {
                infoViewModel.Value.RaiseChanges();
            }

            this.driveInfoService.Update();
        }

        #region < Relay Commands >

        private void InitRelay() => this.CloseCommand = new RelayCommand(this.Close);

        private void Close(object param)
        {
            //Application.Current.Shutdown();
            var window = param as Window;
            window?.Close();
        }

        public RelayCommand CloseCommand { get; set; }

        #endregion < Relay Commands >
    }
}

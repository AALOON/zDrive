using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using zDrive.Converters;
using zDrive.Interfaces;
using zDrive.Mvvm;

namespace zDrive.ViewModels
{
    /// <summary>
    ///     VM for main windows.
    /// </summary>
    internal sealed class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IDriveDetectionService _detectionService;
        private readonly IDriveInfoService _driveInfoService;
        private readonly IRegistryService _registryService;

        private bool _showUnavailable;
        private bool _topmost;
        private double _x, _y;
        private Theme _theme;

        public MainViewModel(IRegistryService registryService,
            IDriveInfoService driveInfoService,
            IDriveDetectionService detectionService,
            IWidgetsService widgetsService,
            ITimerService timerService,
            IDictionary<string, IDriveViewModel> drives,
            IDictionary<string, IInfoViewModel> widgets)
        {
            _registryService = registryService;
            _driveInfoService = driveInfoService;
            _detectionService = detectionService;
            Drives = drives;
            Widgets = widgets;

            _detectionService.DeviceAdded += DeviceAdded;
            _detectionService.DeviceRemoved += DeviceRemoved;

            timerService.Tick += TimerTick;

            InitRelay();

            Initialize();

            widgetsService.Add(InfoWidget.RamDisk);
            if (Screen.AllScreens.Length > 1) widgetsService.Add(InfoWidget.Displays);
        }

        /// <inheritdoc />
        public IDictionary<string, IInfoViewModel> Widgets { get; }

        /// <inheritdoc />
        public IDictionary<string, IDriveViewModel> Drives { get; }

        /// <inheritdoc />
        public bool ShowUnavailable
        {
            get => _showUnavailable;
            set
            {
                if (Set(ref _showUnavailable, value))
                {
                    _driveInfoService.ShowUnavailable = value;
                    _registryService.Write("ShowUnavailable", value);
                }
            }
        }

        /// <inheritdoc />
        public bool Topmost
        {
            get => _topmost;
            set
            {
                if (Set(ref _topmost, value))
                    _registryService.Write("Topmost", value);
            }
        }

        /// <inheritdoc />
        public double X
        {
            get => _x;
            set
            {
                if (Set(ref _x, value))
                    _registryService.Write(nameof(X), value);
            }
        }

        /// <inheritdoc />
        public double Y
        {
            get => _y;
            set
            {
                if (Set(ref _y, value))
                    _registryService.Write(nameof(Y), value);
            }
        }

        /// <inheritdoc />
        public bool AutoRun
        {
            get => _registryService.ReadAutoRun() != null;
            set
            {
                if (value)
                {
                    const string dll = ".dll";
                    const string exe = ".exe";

                    var location = Assembly.GetExecutingAssembly().Location;
                    if (location.EndsWith(dll))
                        location = location.Substring(0, location.Length - dll.Length) + exe;

                    _registryService.WriteAutoRun(location);
                }
                else
                    _registryService.RemoveAutoRun();
                RaisePropertyChanged(nameof(AutoRun));
            }
        }

        /// <inheritdoc />
        public InfoFormat InfoFormat
        {
            get => _driveInfoService.InfoFormat;
            set
            {
                _driveInfoService.InfoFormat = value;

                _registryService.Write(nameof(InfoFormat), (int)value);
                RaisePropertyChanged(nameof(InfoFormat));
            }
        }

        /// <inheritdoc />
        public Theme Theme
        {
            get => _theme;
            set
            {
                if (_theme == value)
                    return;

                var app = (App)System.Windows.Application.Current;
                var nonDefault = value;
                if (nonDefault == Theme.Default)
                    nonDefault = Theme.Gray;
                app.ChangeSkin(new Uri($"/Skins/{nonDefault}Skin.xaml", UriKind.RelativeOrAbsolute));
                _theme = value;
                _registryService.Write(nameof(Theme), (int)value);
                RaisePropertyChanged(nameof(Theme));
            }
        }

        /// <inheritdoc />
        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return _detectionService.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }

        private void Initialize()
        {
            // initialize settings from registry
            _topmost = _registryService.Read(nameof(Topmost), false);
            _showUnavailable = _registryService.Read(nameof(ShowUnavailable), false);
            InfoFormat = _registryService.Read(nameof(InfoFormat), InfoFormat.Free);
            Theme = _registryService.Read(nameof(Theme), Theme.Default);

            X = _registryService.Read(nameof(X), 0D);
            Y = _registryService.Read(nameof(Y), 0D);

            if (Math.Abs(X) < 0.1)
                X = SystemParameters.WorkArea.Width - 180;
            if (Math.Abs(Y) < 0.1)
                Y = 0D;
        }

        private void DeviceAdded(object sender, DeviceArrivalEventArgs e)
        {
            _driveInfoService.UpdateAddition(e.Volume);
        }

        private void DeviceRemoved(object sender, DeviceRemovalEventArgs e)
        {
            _driveInfoService.UpdateRemoval(e.Volume);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            foreach (var infoViewModel in Widgets) infoViewModel.Value.RaiseChanges();

            _driveInfoService.Update();
        }

        #region < Relay Commands >

        private void InitRelay()
        {
            CloseCommand = new RelayCommand(Close);
        }

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
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

        public MainViewModel(IRegistryService registryService,
            IDriveInfoService driveInfoService,
            IDriveDetectionService detectionService,
            IInfosService infosService,
            IInfoFormatService infoFormatService,
            ITimerService timerService,
            IDictionary<string, IDriveViewModel> drives,
            ICollection<IInfoViewModel> infos)
        {
            _registryService = registryService;
            _driveInfoService = driveInfoService;
            _detectionService = detectionService;
            Drives = drives;
            Infos = infos;

            _detectionService.DeviceAdded += DeviceAdded;
            _detectionService.DeviceRemoved += DeviceRemoved;

            timerService.Tick += TimerTick;

            InitRelay();

            Initialize();

            infosService.Add(InfoWidget.RamDisk);
            if (Screen.AllScreens.Length > 1) infosService.Add(InfoWidget.Displays);
        }

        public ICollection<IInfoViewModel> Infos { get; }
        public IDictionary<string, IDriveViewModel> Drives { get; }

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

        public bool Topmost
        {
            get => _topmost;
            set
            {
                if (Set(ref _topmost, value))
                    _registryService.Write("Topmost", value);
            }
        }

        public double X
        {
            get => _x;
            set
            {
                if (Set(ref _x, value))
                    _registryService.Write(nameof(X), value);
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                if (Set(ref _y, value))
                    _registryService.Write(nameof(Y), value);
            }
        }

        public bool AutoRun
        {
            get => _registryService.ReadAutoRun() != null;
            set
            {
                if (value)
                    _registryService.WriteAutoRun(Assembly.GetExecutingAssembly().Location);
                else
                    _registryService.RemoveAutoRun();
                RaisePropertyChanged(nameof(AutoRun));
            }
        }

        public InfoFormat InfoFormat
        {
            get => _driveInfoService.InfoFormat;
            set
            {
                _driveInfoService.InfoFormat = value;

                _registryService.Write(nameof(InfoFormat), (int) value);
                RaisePropertyChanged(nameof(InfoFormat));
            }
        }

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return _detectionService.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }

        private void Initialize()
        {
            // initialize settings from registry
            _topmost = Convert.ToBoolean(_registryService.Read(nameof(Topmost), false));
            _showUnavailable = Convert.ToBoolean(_registryService.Read(nameof(ShowUnavailable), false));
            InfoFormat = (InfoFormat) Convert.ToInt32(_registryService.Read(nameof(InfoFormat), InfoFormat.Free));

            X = Convert.ToDouble(_registryService.Read(nameof(X), 0D));
            Y = Convert.ToDouble(_registryService.Read(nameof(Y), 0D));

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
            foreach (var infoViewModel in Infos) infoViewModel.RaiseChanges();

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
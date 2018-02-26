using System;
using zDrive.Helpers;
using System.Windows;
using System.Collections.ObjectModel;
using System.Timers;
using System.Threading.Tasks;
using zDrive.Interfaces;

namespace zDrive.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        #region < Private Fields >

        private readonly ObservableCollection<DriveViewModel> _drives;

        private readonly IRegistryService _registryService;
        private readonly IDriveInfoService _driveInfoService;

        private bool _showUnavailable;
        private bool _topmost;
        private double _x, _y;

        #endregion < Private Fields >

        #region < Properties >

        public ObservableCollection<DriveViewModel> Drives => _drives;

        public bool ShowUnavailable
        {
            get => _showUnavailable;
            set
            {
                if (Set(ref _showUnavailable, value))
                {
                    _driveInfoService.ShowUnavailable = value;
                    _registryService.Write("ShowUnavailable", value);
                    CheckDisks();
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
                    _registryService.WriteAutoRun(System.Reflection.Assembly.GetExecutingAssembly().Location);
                else
                    _registryService.Remove();
                RaisePropertyChanged(nameof(AutoRun));
            }
        }
        #endregion < Properties >

        private readonly Timer _timer = new Timer(10000);

        public MainViewModel(IRegistryService registryService, IDriveInfoService driveInfoService)
        {
            _drives = new ObservableCollection<DriveViewModel>();
            _registryService = registryService;
            _driveInfoService = driveInfoService;
            _driveInfoService.Drives = _drives;

            InitSetting();
            _timer.Elapsed += timer_Elapsed;
            CheckDisks();
            InitRelay();
        }

        void InitSetting()
        {
            Topmost = Convert.ToBoolean(_registryService.Read("Topmost", false));
            ShowUnavailable = Convert.ToBoolean(_registryService.Read("ShowUnavailable", false));

            X = Convert.ToDouble(_registryService.Read("X", 0D));
            Y = Convert.ToDouble(_registryService.Read("Y", 0D));

            if (Math.Abs(X) < 0.1)
                X = SystemParameters.WorkArea.Width - 180;
            if (Math.Abs(Y) < 0.1)
                Y = 0D;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckDisks();
        }

        public void CheckDisks()
        {
            _timer.Stop();
            Task.Factory.StartNew(() => _driveInfoService.Update())
                        .ContinueWith(_ => _timer.Start());
        }

        #region < Relay Commands >

        private void InitRelay()
        {
            CloseCommnad = new RelayCommand(Close);
        }
        private void Close(object param)
        {
            Application.Current.Shutdown();
        }

        public RelayCommand CloseCommnad { get; set; }

        #endregion

    }
}

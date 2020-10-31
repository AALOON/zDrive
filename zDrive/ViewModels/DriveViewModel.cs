using System;
using System.Diagnostics;
using System.IO;
using zDrive.Interfaces;
using zDrive.Mvvm;

namespace zDrive.ViewModels
{
    internal class DriveViewModel : ViewModelBase, IDriveViewModel
    {
        private readonly DriveInfo _driveInfo;
        private readonly IInfoFormatter _infoFormatter;

        public DriveViewModel(DriveInfo driveInfo, IInfoFormatter format)
        {
            _driveInfo = driveInfo ?? throw new ArgumentNullException(nameof(driveInfo));
            _infoFormatter = format ?? throw new ArgumentNullException(nameof(format));
            OpenCommand = new RelayCommand(Open);
        }

        public void RaiseChanges()
        {
            RaisePropertyChanged(nameof(Label));
            RaisePropertyChanged(nameof(Format));
            RaisePropertyChanged(nameof(Type));
            RaisePropertyChanged(nameof(TotalSize));
            RaisePropertyChanged(nameof(TotalFreeSpace));
            RaisePropertyChanged(nameof(Info));
            RaisePropertyChanged(nameof(Value));
        }

        public string Key => _driveInfo.Name;
        public string Name => _driveInfo.Name;


        public string Label => _driveInfo.IsReady ? _driveInfo.VolumeLabel : "";
        public string Format => _driveInfo.IsReady ? _driveInfo.DriveFormat : "";
        public DriveType Type => _driveInfo.IsReady ? _driveInfo.DriveType : DriveType.Unknown;
        public long TotalSize => _driveInfo.IsReady ? _driveInfo.TotalSize : 0L;
        public long TotalFreeSpace => _driveInfo.IsReady ? _driveInfo.AvailableFreeSpace : 0L;


        public string Info => _infoFormatter.GetFormatedString(TotalSize, TotalFreeSpace);

        public double Value
        {
            get
            {
                if (!_driveInfo.IsReady)
                    return 0d;

                var free = _driveInfo.AvailableFreeSpace;
                var total = _driveInfo.TotalSize;

                if (total == 0L)
                    return 0d;

                return (total - free) / (total / 100d);
            }
        }


        public RelayCommand OpenCommand { get; }

        private void Open(object param)
        {
            Process.Start(Name);
        }
    }
}
using System;
using System.Diagnostics;
using System.IO;
using zDrive.Interfaces;
using zDrive.Mvvm;

namespace zDrive.ViewModels
{
    internal class DriveViewModel : ViewModelBase, IDriveViewModel
    {
        private readonly DriveInfo driveInfo;
        private readonly IInfoFormatter infoFormatter;

        public DriveViewModel(DriveInfo driveInfo, IInfoFormatter format)
        {
            this.driveInfo = driveInfo ?? throw new ArgumentNullException(nameof(driveInfo));
            this.infoFormatter = format ?? throw new ArgumentNullException(nameof(format));
            this.OpenCommand = new RelayCommand(this.Open);
        }

        public void RaiseChanges()
        {
            this.RaisePropertyChanged(nameof(this.DisplayString));
            this.RaisePropertyChanged(nameof(this.Label));
            this.RaisePropertyChanged(nameof(this.Format));
            this.RaisePropertyChanged(nameof(this.Type));
            this.RaisePropertyChanged(nameof(this.TotalSize));
            this.RaisePropertyChanged(nameof(this.TotalFreeSpace));
            this.RaisePropertyChanged(nameof(this.Info));
            this.RaisePropertyChanged(nameof(this.Value));
        }

        public string Key => this.driveInfo.Name;

        public string Name => this.driveInfo.Name;

        public string DisplayString => this.Name + this.Label;

        public string Label => this.driveInfo.IsReady ? this.driveInfo.VolumeLabel : "";

        public string Format => this.driveInfo.IsReady ? this.driveInfo.DriveFormat : "";

        public DriveType Type => this.driveInfo.IsReady ? this.driveInfo.DriveType : DriveType.Unknown;

        public long TotalSize => this.driveInfo.IsReady ? this.driveInfo.TotalSize : 0L;

        public long TotalFreeSpace => this.driveInfo.IsReady ? this.driveInfo.AvailableFreeSpace : 0L;


        public string Info => this.infoFormatter.GetFormatedString(this.TotalSize, this.TotalFreeSpace);

        public double Value
        {
            get
            {
                if (!this.driveInfo.IsReady)
                {
                    return 0d;
                }

                var free = this.driveInfo.AvailableFreeSpace;
                var total = this.driveInfo.TotalSize;

                if (total == 0L)
                {
                    return 0d;
                }

                return (total - free) / (total / 100d);
            }
        }


        public RelayCommand OpenCommand { get; }

        private void Open(object param) => Process.Start("explorer.exe", this.Name);
    }
}

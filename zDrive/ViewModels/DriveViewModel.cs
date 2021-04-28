using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using zDrive.Interfaces;
using zDrive.Mvvm;
using zDrive.Native;
using zDrive.Native.Shell;

namespace zDrive.ViewModels
{
    /// <summary>
    /// Disk drive info view model.
    /// </summary>
    internal class DriveViewModel : ViewModelBase, IDriveViewModel
    {
        private readonly DriveInfo driveInfo;
        private readonly IInfoFormatter infoFormatter;
        private readonly IPdhCounter counter;

        public DriveViewModel(DriveInfo driveInfo, IInfoFormatter format, ILoggerFactory loggerFactory)
        {
            this.driveInfo = driveInfo ?? throw new ArgumentNullException(nameof(driveInfo));
            this.infoFormatter = format ?? throw new ArgumentNullException(nameof(format));
            this.LeftMouseCommand = new RelayCommand(this.Open);
            this.RightMouseCommand = new RelayCommand(this.Properties);
            var label = driveInfo.Name;
            if (label.EndsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                label = label[0..^1];
                this.counter = new PdhCounter($@"\LogicalDisk({label})\Disk Read Bytes/sec", loggerFactory);
            }
            this.counter.InitializeCounters();
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

        /// <inheritdoc />
        public RelayCommand RightMouseCommand { get; }

        public string Key => this.driveInfo.Name;

        public string Name => this.driveInfo.Name;

        public string DisplayString => this.Name + this.Label;

        public string Label => this.driveInfo.IsReady ? this.driveInfo.VolumeLabel : "";

        public string Format => this.driveInfo.IsReady ? this.driveInfo.DriveFormat : "";

        public DriveType Type => this.driveInfo.IsReady ? this.driveInfo.DriveType : DriveType.Unknown;

        public long TotalSize => this.driveInfo.IsReady ? this.driveInfo.TotalSize : 0L;

        public long TotalFreeSpace => this.driveInfo.IsReady ? this.driveInfo.AvailableFreeSpace : 0L;


        public string Info => this.infoFormatter.GetFormatedString(this.TotalSize, this.TotalFreeSpace) + $" {this.counter?.Collect()}";

        public double Value
        {
            get
            {
                if (!this.driveInfo.IsReady)
                {
                    return 0d;
                }

                var freeBytes = this.driveInfo.AvailableFreeSpace;
                var totalByes = this.driveInfo.TotalSize;

                if (totalByes == 0L)
                {
                    return 0d;
                }

                return (totalByes - freeBytes) / (totalByes / 100d);
            }
        }


        public RelayCommand LeftMouseCommand { get; }

        private void Open(object param) => Task.Run(() => Process.Start("explorer.exe", this.Name));

        private void Properties(object param)
        {
            if (string.IsNullOrEmpty(this.Key))
            {
                return;
            }

            Task.Run(() => ShellExecuteApi.ShowFileProperties(this.Key));
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using zDrive.Interfaces;
using zDrive.Mvvm;
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
        private string info = string.Empty;
        private long totalFreeSpace;
        private long totalSize;
        private double value;

        public DriveViewModel(DriveInfo driveInfo, IInfoFormatter format)
        {
            this.driveInfo = driveInfo ?? throw new ArgumentNullException(nameof(driveInfo));
            this.infoFormatter = format ?? throw new ArgumentNullException(nameof(format));
            this.LeftMouseCommand = new RelayCommand(this.Open);
            this.RightMouseCommand = new RelayCommand(this.Properties);
        }

        public void RaiseChanges()
        {
            this.UpdateValues();

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

        public long TotalSize => this.totalSize;

        public long TotalFreeSpace => this.totalFreeSpace;

        public string Info => this.info;

        public double Value => this.value;

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

        private void UpdateValues()
        {
            try
            {
                this.totalSize = this.driveInfo.IsReady ? this.driveInfo.TotalSize : 0L;
                this.totalFreeSpace = this.driveInfo.IsReady ? this.driveInfo.AvailableFreeSpace : 0L;
            }
            catch (Exception)
            {
                // Drive became unavailable between checks - keep the last known values.
                return;
            }

            this.info = this.infoFormatter.GetFormatedString(this.totalSize, this.totalFreeSpace);
            this.value = GetValue(this.totalSize, this.totalFreeSpace);
        }

        private static double GetValue(long total, long free)
        {
            if (total == 0L)
            {
                return 0d;
            }

            return (total - free) / (total / 100d);
        }
    }
}

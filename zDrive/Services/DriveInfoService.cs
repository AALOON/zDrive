using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using zDrive.Interfaces;
using zDrive.ViewModels;

namespace zDrive.Services
{
    internal class DriveInfoService : IDriveInfoService
    {
        private readonly Func<DriveInfo, string> _keyFunc = x => x.Name + x.VolumeLabel + x.DriveFormat;

        public bool ShowUnavailable { get; set; }
        public IList<DriveViewModel> Drives { get; set; }

        private DriveInfo[] GetDrives()
        {
            return DriveInfo.GetDrives();
        }
        private IDictionary<string, DriveInfo> GetDrivesDictionary(DriveInfo[] driveInfos)
        {
            return driveInfos.ToDictionary(_keyFunc);
        }

        private DriveViewModel GetDriveViewModel(DriveInfo drive)
        {
            return new DriveViewModel
            {
                Key = _keyFunc(drive),
                Name = drive.Name,
                Type = drive.DriveType,
                Label = drive.VolumeLabel,
                Format = drive.DriveFormat,
                TotalSize = drive.TotalSize,
                TotalFreeSpace = drive.TotalFreeSpace
            };
        }

        void MainThreadOperation(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
        }

        public void Update()
        {
            if(Drives == null)
                throw new NullReferenceException(nameof(Drives));

            if (Drives.Count != 0)
                UpdateNotEmpty();
            else
            {
                foreach (var item in GetDrives())
                {
                    if (ShowUnavailable)
                        if (!item.IsReady)
                            continue;

                    MainThreadOperation(() => Drives.Add(GetDriveViewModel(item)));
                }
            }
        }

        void UpdateNotEmpty()
        {
            var driveInfosDictionary = GetDrivesDictionary(GetDrives());

            var needRemove = new List<int>(Drives.Count);
            for (var i = 0; i < Drives.Count; i++)
            {
                var drive = Drives[i];
                if (driveInfosDictionary.ContainsKey(drive.Key))
                {
                    var driveInfo = driveInfosDictionary[drive.Key];

                    drive.Label = driveInfo.VolumeLabel;
                    drive.Format = driveInfo.DriveFormat;
                    drive.TotalSize = driveInfo.TotalSize;
                    drive.TotalFreeSpace = driveInfo.TotalFreeSpace;

                    driveInfosDictionary.Remove(drive.Key);
                }
                else
                {
                    needRemove.Add(i);
                }
            }

            foreach (var i in needRemove.OrderByDescending(x => x))
                MainThreadOperation(() => Drives.RemoveAt(i));

            foreach (var item in driveInfosDictionary.Values)
                MainThreadOperation(() => Drives.Add(GetDriveViewModel(item)));
        }
    }
}

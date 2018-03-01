using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using zDrive.Converters;
using zDrive.Interfaces;
using zDrive.ViewModels;

namespace zDrive.Services
{
    internal class DriveInfoService : IDriveInfoService
    {
        private readonly object _lock = new object();
        private readonly Func<DriveInfo, string> _keyFunc = x => x.Name + x.VolumeLabel + x.DriveFormat;
        private InfoFormat _infoFormat = InfoFormat.Free;

        public bool ShowUnavailable { get; set; }

        public InfoFormat InfoFormat
        {
            get => _infoFormat;
            set
            {
                _infoFormat = value;
                foreach (var driveViewModel in Drives)
                    driveViewModel.UpdateInfo(_infoFormat);
            }
        }

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
            if (drive.IsReady)
                return new DriveViewModel(_infoFormat)
                {
                    Key = _keyFunc(drive),
                    Name = drive.Name,
                    Type = drive.DriveType,
                    Label = drive.VolumeLabel,
                    Format = drive.DriveFormat,
                    TotalSize = drive.TotalSize,
                    TotalFreeSpace = drive.TotalFreeSpace
                };
            return new DriveViewModel(_infoFormat)
            {
                Name = drive.Name,
                Type = drive.DriveType
            };
        }

        void MainThreadOperation(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
        }

        public void Update()
        {
            if (Drives == null)
                throw new NullReferenceException(nameof(Drives));

            lock (_lock)
            {
                if (Drives.Count != 0)
                    UpdateNotEmpty();
                else
                {
                    foreach (var item in GetDrives())
                    {
                        if (!ShowUnavailable && !item.IsReady)
                            continue;

                        MainThreadOperation(() => Drives.Add(GetDriveViewModel(item)));
                    }
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

                    if (driveInfo.IsReady)
                    {
                        drive.Label = driveInfo.VolumeLabel;
                        drive.Format = driveInfo.DriveFormat;
                        drive.TotalSize = driveInfo.TotalSize;
                        drive.TotalFreeSpace = driveInfo.TotalFreeSpace;
                    }
                    else
                    {
                        if (!ShowUnavailable)
                            needRemove.Add(i);
                    }

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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using zDrive.Converters;
using zDrive.Interfaces;
using zDrive.ViewModels;

namespace zDrive.Services
{
    internal class DriveInfoService : IDriveInfoService
    {
        private readonly Func<DriveInfo, string> _keyFunc = x => x.Name;
        private readonly IInfoFormatService _infoFormatter;


        protected IDictionary<string, IDriveViewModel> Drives { get; set; }


        public DriveInfoService(IDictionary<string, IDriveViewModel> dictionary, IInfoFormatService infoFormatter)
        {
            _infoFormatter = infoFormatter;
            Drives = dictionary;
            Initialize();
        }


        private DriveInfo[] GetDrives()
        {
            return DriveInfo.GetDrives();
        }

        private void Insert(DriveInfo driveInfo)
        {
            Drives.Add(_keyFunc(driveInfo), new DriveViewModel(driveInfo, _infoFormatter));
        }

        private void Initialize()
        {
            foreach (var driveInfo in GetDrives())
            {
                if (driveInfo.IsReady || ShowUnavailable)
                {
                    Insert(driveInfo);
                }
            }
        }


        public bool ShowUnavailable { get; set; }

        public InfoFormat InfoFormat
        {
            get => _infoFormatter.Format;
            set
            {
                if (_infoFormatter.Format != value)
                {
                    _infoFormatter.Format = value;
                    Update();
                }
            }
        }
        
        public void Update()
        {
            foreach (var infoViewModel in Drives)
                infoViewModel.Value.RaiseChanges();
        }

        public void UpdateAddition(string label)
        {
            UpdateRemoval(label);
            var driveInfo = new DriveInfo(label);
            Insert(driveInfo);
        }

        public void UpdateRemoval(string label)
        {
            Debug.Assert(!string.IsNullOrEmpty(label), "!string.IsNullOrEmpty(label)");

            if (Drives.ContainsKey(label))
            {
                Drives.Remove(label);
            }
        }
    }
}

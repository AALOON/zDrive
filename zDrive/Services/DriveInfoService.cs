using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using zDrive.Converters;
using zDrive.Interfaces;
using zDrive.ViewModels;

namespace zDrive.Services
{
    internal class DriveInfoService : IDriveInfoService
    {
        private readonly IInfoFormatService infoFormatter;
        private readonly ILoggerFactory loggerFactory;
        private readonly Func<DriveInfo, string> keyFunc = x => x.Name;


        public DriveInfoService(IDictionary<string, IDriveViewModel> dictionary, IInfoFormatService infoFormatter,
            ILoggerFactory loggerFactory)
        {
            this.infoFormatter = infoFormatter;
            this.loggerFactory = loggerFactory;
            this.Drives = dictionary;
            this.Initialize();
        }


        protected IDictionary<string, IDriveViewModel> Drives { get; set; }


        public bool ShowUnavailable { get; set; }

        public InfoFormat InfoFormat
        {
            get => this.infoFormatter.Format;
            set
            {
                if (this.infoFormatter.Format != value)
                {
                    this.infoFormatter.Format = value;
                    this.Update();
                }
            }
        }

        public void Update()
        {
            foreach (var infoViewModel in this.Drives)
            {
                infoViewModel.Value.RaiseChanges();
            }
        }

        public void UpdateAddition(string label)
        {
            this.UpdateRemoval(label);
            var driveInfo = new DriveInfo(label);
            this.Insert(driveInfo);
        }

        public void UpdateRemoval(string label)
        {
            if (!string.IsNullOrEmpty(label) && this.Drives.ContainsKey(label))
            {
                this.Drives.Remove(label);
            }
        }


        private static DriveInfo[] GetDrives() => DriveInfo.GetDrives();

        private void Initialize()
        {
            foreach (var driveInfo in GetDrives())
            {
                if (driveInfo.IsReady || this.ShowUnavailable)
                {
                    this.Insert(driveInfo);
                }
            }
        }

        private void Insert(DriveInfo driveInfo) =>
            this.Drives.Add(this.keyFunc(driveInfo), new DriveViewModel(driveInfo, this.infoFormatter, this.loggerFactory));
    }
}

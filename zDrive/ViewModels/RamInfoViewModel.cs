using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using zDrive.Interfaces;
using zDrive.Mvvm;
using zDrive.Native.Ps;

namespace zDrive.ViewModels
{
    internal sealed class RamInfoViewModel : ViewModelBase, IInfoViewModel
    {
        private const string TaskManager = "Taskmgr";
        private readonly IInfoFormatter format;
        private readonly ILogger<RamInfoViewModel> logger;

        internal RamInfoViewModel(IInfoFormatter format, ILogger<RamInfoViewModel> logger)
        {
            this.format = format;
            this.logger = logger;
            this.LeftMouseCommand = new RelayCommand(this.Open);
            this.RightMouseCommand = new RelayCommand(this.Open);
        }

        public long Total { get; private set; }

        public long Free { get; private set; }

        /// <inheritdoc />
        public RelayCommand LeftMouseCommand { get; }

        /// <inheritdoc />
        public RelayCommand RightMouseCommand { get; }

        public string Key => "RamInfo";

        public string Name => "Ram usage";

        public string DisplayString => this.Name;

        public string Info { get; private set; }

        public double Value
        {
            get
            {
                var free = this.Free;
                var total = this.Total;

                if (total == 0L)
                {
                    return 0d;
                }

                return (total - free) / (total / 100d);
            }
        }

        public void RaiseChanges()
        {
            try
            {
                var pi = new PerformanceInformation();
                if (PsApi.GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    this.Free = Convert.ToInt64(pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64());
                    this.Total = Convert.ToInt64(pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64());
                }
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Error on resolving ram info.");
            }

            this.Info = this.format.GetFormatedString(this.Total, this.Free);

            this.RaisePropertyChanged(nameof(this.Total));
            this.RaisePropertyChanged(nameof(this.Free));
            this.RaisePropertyChanged(nameof(this.Info));
            this.RaisePropertyChanged(nameof(this.Value));
        }

        private void Open(object param)
        {
            var process = new ProcessStartInfo(TaskManager) { UseShellExecute = true };

            _ = Process.Start(process);
        }
    }
}

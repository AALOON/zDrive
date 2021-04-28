using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using zDrive.Interfaces;
using zDrive.Mvvm;
using zDrive.Native;

namespace zDrive.ViewModels
{
    internal sealed class CpuInfoViewModel : ViewModelBase, IInfoViewModel, IDisposable
    {
        private const string TaskManager = "Taskmgr";
        private readonly IPdhCounter cpuCounter;

        private readonly ILogger<CpuInfoViewModel> logger;

        internal CpuInfoViewModel(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<CpuInfoViewModel>();
            this.LeftMouseCommand = new RelayCommand(this.Open);
            this.RightMouseCommand = new RelayCommand(this.Open);

            this.cpuCounter = new PdhCounter(@"\Processor(_Total)\% Processor Time", loggerFactory);
            this.cpuCounter.InitializeCounters();
        }

        /// <inheritdoc />
        public RelayCommand LeftMouseCommand { get; }

        /// <inheritdoc />
        public RelayCommand RightMouseCommand { get; }

        public string Key => "CpuInfo";

        public string Name => "Cpu usage";

        public string DisplayString => this.Name;

        public string Info { get; private set; }

        public double Value { get; private set; }

        public void RaiseChanges()
        {
            if (!this.TryUpdateValue())
            {
                return;
            }

            this.Info = $"{this.Value:F1} %";

            this.RaisePropertyChanged(nameof(this.Info));
            this.RaisePropertyChanged(nameof(this.Value));
        }


        private void Open(object param)
        {
            var process = new ProcessStartInfo(TaskManager) { UseShellExecute = true };

            _ = Process.Start(process);
        }


        private bool TryUpdateValue()
        {
            try
            {
                this.Value = this.cpuCounter.Collect() ?? this.Value;
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Error on resolving ram info.");
            }

            return true;
        }

        /// <inheritdoc />
        public void Dispose() => this.cpuCounter?.Dispose();
    }
}

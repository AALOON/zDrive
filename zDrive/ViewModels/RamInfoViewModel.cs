using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using zDrive.Interfaces;
using zDrive.Mvvm;

namespace zDrive.ViewModels
{
    internal class RamInfoViewModel : ViewModelBase, IInfoViewModel
    {
        private const string TaskManager = "Taskmgr";
        private readonly IInfoFormatter format;

        internal RamInfoViewModel(IInfoFormatter format)
        {
            this.format = format;
            this.LeftMouseCommand = new RelayCommand(this.Open);
            this.RightMouseCommand = new RelayCommand(this.Open);
        }

        public long Total { get; private set; }

        public long Free { get; private set; }

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
            this.Free = PerformanceInfo.GetPhysicalAvailableMemory();
            this.Total = PerformanceInfo.GetTotalMemory();

            this.Info = this.format.GetFormatedString(this.Total, this.Free);

            this.RaisePropertyChanged(nameof(this.Total));
            this.RaisePropertyChanged(nameof(this.Free));
            this.RaisePropertyChanged(nameof(this.Info));
            this.RaisePropertyChanged(nameof(this.Value));
        }

        public RelayCommand LeftMouseCommand { get; }

        private void Open(object param) => Process.Start(TaskManager);

        public static class PerformanceInfo
        {
            [DllImport("psapi.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetPerformanceInfo([Out] out PerformanceInformation performanceInformation,
                [In] int size);

            public static long GetPhysicalAvailableMemory()
            {
                var pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64(pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64());
                }

                return -1;
            }

            public static long GetTotalMemory()
            {
                var pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64(pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64());
                }

                return -1;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct PerformanceInformation
            {
                public int Size;
                public IntPtr CommitTotal;
                public IntPtr CommitLimit;
                public IntPtr CommitPeak;
                public IntPtr PhysicalTotal;
                public IntPtr PhysicalAvailable;
                public IntPtr SystemCache;
                public IntPtr KernelTotal;
                public IntPtr KernelPaged;
                public IntPtr KernelNonPaged;
                public IntPtr PageSize;
                public int HandlesCount;
                public int ProcessCount;
                public int ThreadCount;
            }
        }
    }
}

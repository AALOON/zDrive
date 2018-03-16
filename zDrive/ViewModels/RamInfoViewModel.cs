using System;
using System.Runtime.InteropServices;
using zDrive.Helpers;
using zDrive.Interfaces;

namespace zDrive.ViewModels
{
    internal class RamInfoViewModel : ViewModelBase, IInfoViewModel
    {
        private const string TaskManager = "Taskmgr";
        private readonly IInfoFormatter _format;
        internal RamInfoViewModel(IInfoFormatter format)
        {
            _format = format;
            OpenCommand = new RelayCommand(Open);
        }

        public string Key => "RamInfo";
        public string Name => "Ram usage";
        public string Info { get; private set; }
        public double Value
        {
            get
            {
                var free = Free;
                var total = Total;

                if (total == 0L)
                    return 0d;

                return (total - free) / (total / 100d);
            }
        }

        public long Total { get; private set; }
        public long Free { get; private set; }

        public void RaiseChanges()
        {
            Free = PerformanceInfo.GetPhysicalAvailableMemory();
            Total = PerformanceInfo.GetTotalMemory();

            Info = _format.GetFormatedString(Total,Free);

            RaisePropertyChanged(nameof(Total));
            RaisePropertyChanged(nameof(Free));
            RaisePropertyChanged(nameof(Info));
            RaisePropertyChanged(nameof(Value));
        }

        public RelayCommand OpenCommand { get; }
        void Open(object param)
        {
            System.Diagnostics.Process.Start(TaskManager);
        }

        public static class PerformanceInfo
        {
            [DllImport("psapi.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetPerformanceInfo([Out] out PerformanceInformation performanceInformation, [In] int size);

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

            public static long GetPhysicalAvailableMemory()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64()));
                }
                return -1;
            }

            public static long GetTotalMemory()
            {
                PerformanceInformation pi = new PerformanceInformation();
                if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
                {
                    return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64()));
                }
                return -1;
            }
        }
    }
}
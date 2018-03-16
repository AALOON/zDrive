using System;
using System.Collections.Generic;
using zDrive.Converters;
using zDrive.Helpers;

namespace zDrive.Interfaces
{
    internal interface IMainViewModel
    {
        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);

        RelayCommand CloseCommnad { get; set; }

        ICollection<IInfoViewModel> Infos { get; }
        IDictionary<string, IDriveViewModel> Drives { get; }

        bool ShowUnavailable { get; set; }

        bool Topmost { get; set; }

        double X { get; set; }

        double Y { get; set; }

        bool AutoRun { get; set; }

        InfoFormat InfoFormat { get; set; }
    }
}
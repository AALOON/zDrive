using System;
using System.Collections.Generic;
using zDrive.Converters;
using zDrive.Mvvm;

namespace zDrive.Interfaces
{
    /// <summary>
    ///     Main view model.
    /// </summary>
    internal interface IMainViewModel
    {
        /// <summary>
        ///     Close window command.
        /// </summary>
        RelayCommand CloseCommand { get; set; }

        /// <summary>
        ///     Widgets collection.
        /// </summary>
        IDictionary<string, IInfoViewModel> Widgets { get; }

        /// <summary>
        ///     Drives collection.
        /// </summary>
        IDictionary<string, IDriveViewModel> Drives { get; }

        /// <summary>
        ///     Show unavailable disks
        /// </summary>
        bool ShowUnavailable { get; set; }

        /// <summary>
        ///     Windows on top of all windows.
        /// </summary>
        bool Topmost { get; set; }

        /// <summary>
        ///     Location of X of window.
        /// </summary>
        double X { get; set; }

        /// <summary>
        ///     Location of Y of window.
        /// </summary>
        double Y { get; set; }

        /// <summary>
        ///     Auto run app on windows start.
        /// </summary>
        bool AutoRun { get; set; }

        /// <summary>
        ///     Format of info.
        /// </summary>
        InfoFormat InfoFormat { get; set; }

        /// <summary>
        ///     Windows proc.
        /// </summary>
        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);
    }
}
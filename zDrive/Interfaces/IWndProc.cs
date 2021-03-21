using System;

namespace zDrive.Interfaces
{
    public interface IWndProc
    {
        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);
    }
}

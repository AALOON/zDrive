using System;

namespace zDrive.Interfaces
{
    internal interface ITimerService
    {
        event EventHandler<EventArgs> Tick;
    }
}
using System;

namespace zDrive.Interfaces
{
    interface ITimerService
    {
        event EventHandler<EventArgs> Tick;
    }
}

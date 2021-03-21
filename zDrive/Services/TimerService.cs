using System;
using System.Windows.Threading;
using zDrive.Interfaces;

namespace zDrive.Services
{
    internal class TimerService : ITimerService
    {
        private readonly TimeSpan interval = TimeSpan.FromSeconds(2d);

        public TimerService()
        {
            var timer = new DispatcherTimer { Interval = this.interval };
            timer.Tick += this.Timer_Tick;
            timer.Start();
        }

        public event EventHandler<EventArgs> Tick;

        private void Timer_Tick(object sender, EventArgs e) => this.Tick?.Invoke(this, e);
    }
}

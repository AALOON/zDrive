using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace zDrive
{
    public static class WpfSingleInstance
    {
        private static DispatcherTimer _autoExitAplicationIfStartupDeadlock;

        /// <summary>
        ///     Processing single instance.
        /// </summary>
        /// <param name="singleInstanceModes"></param>
        internal static void Make(SingleInstanceModes singleInstanceModes = SingleInstanceModes.ForEveryUser)
        {
            var appName = Application.Current.GetType().Assembly.ManifestModule.ScopeName;

            var windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity.User == null) throw new IdentityNotMappedException();
            var keyUserName = windowsIdentity.User.ToString();

            // Max 260 chars
            var eventWaitHandleName =
                $"{appName}{(singleInstanceModes == SingleInstanceModes.ForEveryUser ? keyUserName : string.Empty)}";

            try
            {
                using (var eventWaitHandle = EventWaitHandle.OpenExisting(eventWaitHandleName))
                {
                    // It informs first instance about other startup attempting.
                    eventWaitHandle.Set();
                }

                // Let's terminate this posterior startup.
                // For that exit no interceptions.
                Environment.Exit(0);
            }
            catch
            {
                // It's first instance.

                // Register EventWaitHandle.
                using (var eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, eventWaitHandleName))
                {
                    ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, OtherInstanceAttemptedToStart, null,
                        Timeout.Infinite, false);
                }

                RemoveApplicationsStartupDeadlockForStartupCrushedWindows();
            }
        }

        private static void OtherInstanceAttemptedToStart(object state, bool timedOut)
        {
            RemoveApplicationsStartupDeadlockForStartupCrushedWindows();
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (Application.Current.MainWindow != null) Application.Current.MainWindow.Activate();
            }));
        }

        /// <summary>
        ///     Бывают случаи, когда при старте произошла ошибка и ни одно окно не появилось.
        ///     При этом второй инстанс приложения уже не запустить, а этот не закрыть, кроме как через панель задач. Deedlock
        ///     своего рода получился.
        /// </summary>
        private static void RemoveApplicationsStartupDeadlockForStartupCrushedWindows()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _autoExitAplicationIfStartupDeadlock =
                        new DispatcherTimer(
                            TimeSpan.FromSeconds(6),
                            DispatcherPriority.ApplicationIdle,
                            (o, args) =>
                            {
                                if (Application.Current.Windows.Cast<Window>()
                                        .Count(window => !double.IsNaN(window.Left)) == 0)
                                    // For that exit no interceptions.
                                    Environment.Exit(0);
                            },
                            Application.Current.Dispatcher
                        );
                }),
                DispatcherPriority.ApplicationIdle
            );
        }
    }

    public enum SingleInstanceModes
    {
        /// <summary>
        ///     Do nothing.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Every user can have own single instance.
        /// </summary>
        ForEveryUser
    }
}
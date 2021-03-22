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
        private static readonly Lazy<DispatcherTimer> LazyDispatcherTimer = new(() => new DispatcherTimer(
            TimeSpan.FromSeconds(6),
            DispatcherPriority.ApplicationIdle,
            (_, _) =>
            {
                // For that exit no interceptions.
                if (Application.Current.Windows.Cast<Window>().All(window => double.IsNaN(window.Left)))
                {
                    Environment.Exit(0);
                }
            },
            Application.Current.Dispatcher
        ));

        /// <summary>
        /// Processing single instance.
        /// </summary>
        internal static void Make(SingleInstanceModes singleInstanceModes = SingleInstanceModes.ForEveryUser)
        {
            var appName = Application.Current.GetType().Assembly.ManifestModule.ScopeName;

            var windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity.User == null)
            {
                throw new IdentityNotMappedException();
            }

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
                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Activate();
                }
            }));
        }

        /// <summary>
        /// Sometimes there cases when there error on start and there no one windows is appeared.
        /// And second instance of app will not started and current will not be closed.
        /// This method prevents deadlock.
        /// </summary>
        private static void RemoveApplicationsStartupDeadlockForStartupCrushedWindows() =>
            Application.Current.Dispatcher.BeginInvoke(new Action(() => _ = LazyDispatcherTimer.Value),
                DispatcherPriority.ApplicationIdle
            );
    }

    public enum SingleInstanceModes
    {
        /// <summary>
        /// Do nothing.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Every user can have own single instance.
        /// </summary>
        ForEveryUser
    }
}

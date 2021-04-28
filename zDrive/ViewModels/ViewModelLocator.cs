using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using zDrive.Collections;
using zDrive.Interfaces;
using zDrive.Services;
using zDrive.Services.Ioc;

namespace zDrive.ViewModels
{
    /// <summary>
    /// View model locator.
    /// </summary>
    internal sealed class ViewModelLocator
    {
        static ViewModelLocator()
        {
            Ioc.RegisterSingleton(() => LoggerFactory.Create(builder
                => builder.AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddEventLog()));

            // TODO: ioc generic support.
            Ioc.RegisterPerDependency(() => Ioc.Resolve<ILoggerFactory>().CreateLogger(typeof(ViewModelLocator)));

            Ioc.RegisterSingleton<IDictionary<string, IDriveViewModel>>(() =>
                new ObservableDictionary<string, IDriveViewModel>());
            Ioc.RegisterSingleton<IDictionary<string, IInfoViewModel>>(() =>
                new ObservableDictionary<string, IInfoViewModel>(() => new SortedDictionary<string, IInfoViewModel>()));
            Ioc.RegisterSingleton<IRegistryService, RegistryService>();
            Ioc.RegisterSingleton<IDriveDetectionService, IWndProc, DriveDetectionService>();
            Ioc.RegisterSingleton<IWidgetsService, WidgetsService>();
            Ioc.RegisterSingleton<IInfoFormatService, InfoFormatService>();
            Ioc.RegisterSingleton<IDriveInfoService, DriveInfoService>();
            Ioc.RegisterSingleton<ITimerService, TimerService>();
            Ioc.RegisterSingleton<MainViewModel>();
        }

        public static SimpleIoc Ioc { get; } = new();

        /// <summary>
        /// Main window.
        /// </summary>
        public static IMainViewModel Main => Ioc.Resolve<MainViewModel>();

        public static void Cleanup()
        {
        }
    }
}

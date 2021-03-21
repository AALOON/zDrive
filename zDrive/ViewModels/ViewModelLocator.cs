using System.Collections.Generic;
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
            Ioc.RegisterSingleton<IDictionary<string, IDriveViewModel>, ObservableDictionary<string, IDriveViewModel>>(
                () => new ObservableDictionary<string, IDriveViewModel>());
            Ioc.RegisterSingleton<IDictionary<string, IInfoViewModel>, ObservableDictionary<string, IInfoViewModel>>(
                () => new ObservableDictionary<string, IInfoViewModel>());
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

using System.Collections.Generic;
using zDrive.Collections;
using zDrive.Interfaces;
using zDrive.Services;

namespace zDrive.ViewModels
{
    /// <summary>
    ///     View model locator.
    /// </summary>
    internal sealed class ViewModelLocator
    {
        static ViewModelLocator()
        {
            SimpleIoc.RegisterType<IDictionary<string, IDriveViewModel>, ObservableDictionary<string, IDriveViewModel>>(
                () => new ObservableDictionary<string, IDriveViewModel>());
            SimpleIoc.RegisterType<IDictionary<string, IInfoViewModel>, ObservableDictionary<string, IInfoViewModel>>(
                () =>
                    new ObservableDictionary<string, IInfoViewModel>());
            SimpleIoc.RegisterType<IRegistryService, RegistryService>();
            SimpleIoc.RegisterType<IDriveDetectionService, DriveDetectionService>();
            SimpleIoc.RegisterType<IWidgetsService, WidgetsService>();
            SimpleIoc.RegisterType<IInfoFormatService, InfoFormatService>();
            SimpleIoc.RegisterType<IDriveInfoService, DriveInfoService>();
            SimpleIoc.RegisterType<ITimerService, TimerService>();
            SimpleIoc.RegisterType<MainViewModel>();
        }

        /// <summary>
        ///     Main window.
        /// </summary>
        public IMainViewModel Main => SimpleIoc.Resolve<MainViewModel>();

        public static void Cleanup()
        {
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using zDrive.Collections;
using zDrive.Interfaces;
using zDrive.Services;

namespace zDrive.ViewModels
{
    internal class ViewModelLocator
    {
        static ViewModelLocator()
        {
            SimpleIoc.RegisterType<IDictionary<string, IDriveViewModel>, ObservableDictionary<string, IDriveViewModel>>(
                () => new ObservableDictionary<string, IDriveViewModel>());
            SimpleIoc.RegisterType<ICollection<IInfoViewModel>, ObservableCollection<IInfoViewModel>>(() =>
                new ObservableCollection<IInfoViewModel>());
            SimpleIoc.RegisterType<IRegistryService, RegistryService>();
            SimpleIoc.RegisterType<IDriveDetectionService, DriveDetectionService>();
            SimpleIoc.RegisterType<IInfosService, InfosService>();
            SimpleIoc.RegisterType<IInfoFormatService, InfoFormatService>();
            SimpleIoc.RegisterType<IDriveInfoService, DriveInfoService>();
            SimpleIoc.RegisterType<ITimerService, TimerService>();
            SimpleIoc.RegisterType<MainViewModel>();
        }

        public IMainViewModel Main => SimpleIoc.Resolve<MainViewModel>();

        public static void Cleanup()
        {
        }
    }
}
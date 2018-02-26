using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zDrive.Interfaces;
using zDrive.Services;

namespace zDrive.ViewModels
{
    internal class ViewModelLocator
    {
        static ViewModelLocator()
        {
            SimpleIoc.RegisterType<IRegistryService, RegistryService>(() => new RegistryService());
            SimpleIoc.RegisterType<IDriveInfoService, DriveInfoService>(() => new DriveInfoService());
            SimpleIoc.RegisterType<MainViewModel>();
        }

        public MainViewModel Main => new MainViewModel(new RegistryService(), new DriveInfoService());//SimpleIoc.Resolve<MainViewModel>();

        public static void Cleanup()
        {
        }
    }
}

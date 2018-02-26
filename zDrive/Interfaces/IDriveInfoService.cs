using System.Collections.Generic;
using zDrive.ViewModels;

namespace zDrive.Interfaces
{
    internal interface IDriveInfoService
    {
        IList<DriveViewModel> Drives { get; set; }
        bool ShowUnavailable { get; set; }
        void Update();
    }
}

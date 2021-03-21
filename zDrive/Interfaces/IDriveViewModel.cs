using System.IO;

namespace zDrive.Interfaces
{
    internal interface IDriveViewModel : IInfoViewModel
    {
        string Label { get; }

        string Format { get; }

        DriveType Type { get; }

        long TotalSize { get; }

        long TotalFreeSpace { get; }
    }
}

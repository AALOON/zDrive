namespace zDrive.Interfaces
{
    internal interface IDriveViewModel: IInfoViewModel
    {
        string Label { get; }

        string Format { get; }

        System.IO.DriveType Type { get; }

        long TotalSize { get; }

        long TotalFreeSpace { get; }
    }
}
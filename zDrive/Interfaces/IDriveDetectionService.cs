using System;

namespace zDrive.Interfaces
{
    public interface IDriveDetectionService
    {
        event EventHandler<DeviceArrivalEventArgs> DeviceAdded;

        event EventHandler<DeviceRemovalEventArgs> DeviceRemoved;
    }

    #region DriveDetectionService EventArgs classes

    public abstract class DriveDetectionServiceEventArgs : EventArgs
    {
        protected DriveDetectionServiceEventArgs(string volume) => this.Volume = volume;

        public string Volume { get; }
    }

    public sealed class DeviceArrivalEventArgs : DriveDetectionServiceEventArgs
    {
        internal DeviceArrivalEventArgs(string volume) : base(volume)
        {
        }
    }

    public sealed class DeviceRemovalEventArgs : DriveDetectionServiceEventArgs
    {
        internal DeviceRemovalEventArgs(string volume) : base(volume)
        {
        }
    }

    #endregion
}

using System;
using System.Runtime.InteropServices;
using zDrive.Interfaces;

namespace zDrive.Services
{
    public sealed class DriveDetectionService : IDriveDetectionService
    {
        #region Win32 Structures And Consts

        private const int WmDeviceChange = 0x0219;
        private const int DbtDeviceArrival = 0x8000;
        private const int DbtDeviceremovalComplete = 0x8004;
        private const int DbtDevtypVolume = 0x00000002;

        [StructLayout(LayoutKind.Sequential)]
        public struct DevBroadcastVolume
        {
            public int Size;
            public int DeviceType;
            public int Reserved;
            public int Mask;
            public short Flags;

            public char GetLetter()
            {
                const byte lettersCount = 'A' + 26;
                var mask = Mask;
                char c;
                for (c = 'A'; c < lettersCount; c++)
                {
                    if ((mask & 0x1) != 0)
                        break;
                    mask >>= 1;
                }

                return c;
            }

            public char GetLetterBynarySearch()
            {
                var lettersCount = 'A';
                var first = 0;
                var last = 0;
                var one = 1;

                if (Mask != one)
                {
                    last = 26;
                    while (first < last)
                    {
                        var mid = first + (last - first) / 2;

                        if (Mask <= one << mid)
                            last = mid;
                        else
                            first = mid + 1;
                    }
                }

                return (char) (lettersCount + last);
            }
        }

        #endregion

        #region Private methods

        private string GetVolume(IntPtr lParam)
        {
            DevBroadcastVolume? volume = null;
            var devType = Marshal.ReadInt32(lParam, 4);
            if (devType == DbtDevtypVolume)
                volume = (DevBroadcastVolume) Marshal.PtrToStructure(lParam, typeof(DevBroadcastVolume));
            return volume.HasValue ? volume.Value.GetLetter() + ":\\" : "";
        }

        private void OnDeviceArrival(string volume)
        {
            DeviceAdded?.Invoke(this, new DeviceArrivalEventArgs(volume));
        }

        private void OnDeviceRemoval(string volume)
        {
            DeviceRemoved?.Invoke(this, new DeviceRemovalEventArgs(volume));
        }

        #endregion

        #region IDriveDetectionService

        public event EventHandler<DeviceArrivalEventArgs> DeviceAdded;
        public event EventHandler<DeviceRemovalEventArgs> DeviceRemoved;

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            if (msg == WmDeviceChange)
            {
                var eventCode = wParam.ToInt32();

                switch (eventCode)
                {
                    case DbtDeviceArrival:
                        OnDeviceArrival(GetVolume(lParam));
                        break;
                    case DbtDeviceremovalComplete:
                        OnDeviceRemoval(GetVolume(lParam));
                        break;
                }
            }

            return IntPtr.Zero;
        }

        #endregion
    }
}
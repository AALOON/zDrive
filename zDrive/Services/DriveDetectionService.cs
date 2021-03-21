using System;
using System.Runtime.InteropServices;
using zDrive.Interfaces;

namespace zDrive.Services
{
    public sealed class DriveDetectionService : IDriveDetectionService, IWndProc
    {
        #region Win32 Structures And Consts

        private const int WmDeviceChange = 0x0219;
        private const int DbtDeviceArrival = 0x8000;
        private const int DbtDeviceremovalComplete = 0x8004;

        /// <summary>
        /// dbch_devicetype
        /// </summary>
        private enum DeviceType
        {
            DbtDevTypeVolume = 0x00000002,
            DbtDevTypePort = 0x00000003,
            DbtDevTypeOem = 0x00000000,
            DbtDevTypeHandle = 0x00000006,
            DbtDevTypeDeviceInterface = 0x00000005
        }

        public enum DbvcFlags : short
        {
            DBTF_MEDIA = 0x0001,
            DBTF_NET = 0x0002
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DevBroadcastVolume
        {
            public int Size;
            public int DeviceType;
            public int Reserved;
            public int Mask;
            public DbvcFlags Flags;

            public char GetLetter()
            {
                const byte lettersCount = 'A' + 26;
                var mask = this.Mask;
                char c;
                for (c = 'A'; c < lettersCount; c++)
                {
                    if ((mask & 0x1) != 0)
                    {
                        break;
                    }

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

                if (this.Mask != one)
                {
                    last = 26;
                    while (first < last)
                    {
                        var mid = first + ((last - first) / 2);

                        if (this.Mask <= one << mid)
                        {
                            last = mid;
                        }
                        else
                        {
                            first = mid + 1;
                        }
                    }
                }

                return (char)(lettersCount + last);
            }
        }

        #endregion

        #region Private methods

        private string GetVolume(IntPtr lParam)
        {
            var broadcastVolume = (DevBroadcastVolume)Marshal.PtrToStructure(lParam, typeof(DevBroadcastVolume));
            return broadcastVolume.GetLetter() + ":\\";
        }

        private void OnDeviceArrival(string volume) =>
            this.DeviceAdded?.Invoke(this, new DeviceArrivalEventArgs(volume));

        private void OnDeviceRemoval(string volume) =>
            this.DeviceRemoved?.Invoke(this, new DeviceRemovalEventArgs(volume));

        #endregion

        #region IDriveDetectionService

        public event EventHandler<DeviceArrivalEventArgs> DeviceAdded;

        public event EventHandler<DeviceRemovalEventArgs> DeviceRemoved;

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            if (msg != WmDeviceChange)
            {
                return IntPtr.Zero;
            }

            var eventCode = wParam.ToInt32();

            switch (eventCode)
            {
                case DbtDeviceArrival when GetDeviceType(lParam) == DeviceType.DbtDevTypeVolume:
                    this.OnDeviceArrival(this.GetVolume(lParam));
                    break;
                case DbtDeviceremovalComplete when GetDeviceType(lParam) == DeviceType.DbtDevTypeVolume:
                    this.OnDeviceRemoval(this.GetVolume(lParam));
                    break;
            }

            return IntPtr.Zero;
        }

        private static DeviceType GetDeviceType(IntPtr lParam) => (DeviceType)Marshal.ReadInt32(lParam, 4);

        #endregion
    }
}

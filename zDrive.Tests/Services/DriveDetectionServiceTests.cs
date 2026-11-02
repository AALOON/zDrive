using System;
using System.Runtime.InteropServices;
using zDrive.Services;
using Xunit;

namespace zDrive.Tests.Services
{
    public class DriveDetectionServiceTests
    {
        private const int WmDeviceChange = 0x0219;
        private const int DbtDeviceArrival = 0x8000;
        private const int DbtDeviceRemovalComplete = 0x8004;
        private const int DbtDevTypeVolume = 0x00000002;
        private const int DbtDevTypePort = 0x00000003;

        [Theory]
        [InlineData(0x1, 'A')]
        [InlineData(0x2, 'B')]
        [InlineData(0x4, 'C')]
        [InlineData(0x1 << 25, 'Z')]
        public void GetLetter_SingleBitMask_ReturnsCorrespondingLetter(int mask, char expected)
        {
            var volume = CreateVolume(mask);

            Assert.Equal(expected, volume.GetLetter());
        }

        [Fact]
        public void GetLetter_MultiBitMask_ReturnsFirstSetBitLetter()
        {
            // Bits for A and C are set; the first set bit wins.
            var volume = CreateVolume(0x1 | 0x4);

            Assert.Equal('A', volume.GetLetter());
        }

        [Theory]
        [InlineData(0x1, 'A')]
        [InlineData(0x2, 'B')]
        [InlineData(0x4, 'C')]
        [InlineData(0x1 << 25, 'Z')]
        public void GetLetterBynarySearch_SingleBitMask_ReturnsCorrespondingLetter(int mask, char expected)
        {
            var volume = CreateVolume(mask);

            Assert.Equal(expected, volume.GetLetterBynarySearch());
        }

        [Fact]
        public void WndProc_OtherMessage_IsIgnored()
        {
            var service = new DriveDetectionService();
            var raised = 0;
            service.DeviceAdded += (_, _) => raised++;
            service.DeviceRemoved += (_, _) => raised++;
            var handled = false;

            var result = service.WndProc(IntPtr.Zero, 0x0010 /* WM_CLOSE */, IntPtr.Zero, IntPtr.Zero, ref handled);

            Assert.Equal(IntPtr.Zero, result);
            Assert.False(handled);
            Assert.Equal(0, raised);
        }

        [Fact]
        public void WndProc_VolumeArrival_RaisesDeviceAddedWithVolume()
        {
            var service = new DriveDetectionService();
            string added = null;
            service.DeviceAdded += (_, e) => added = e.Volume;
            var handled = false;

            WithVolumePointer(CreateVolume(0x4), ptr =>
                service.WndProc(IntPtr.Zero, WmDeviceChange, new IntPtr(DbtDeviceArrival), ptr, ref handled));

            Assert.Equal("C:\\", added);
        }

        [Fact]
        public void WndProc_VolumeRemoval_RaisesDeviceRemovedWithVolume()
        {
            var service = new DriveDetectionService();
            string removed = null;
            service.DeviceRemoved += (_, e) => removed = e.Volume;
            var handled = false;

            WithVolumePointer(CreateVolume(0x2), ptr =>
                service.WndProc(IntPtr.Zero, WmDeviceChange, new IntPtr(DbtDeviceRemovalComplete), ptr, ref handled));

            Assert.Equal("B:\\", removed);
        }

        [Theory]
        [InlineData(DbtDeviceArrival)]
        [InlineData(DbtDeviceRemovalComplete)]
        public void WndProc_NonVolumeDevice_DoesNotRaiseEvents(int eventCode)
        {
            var service = new DriveDetectionService();
            var raised = 0;
            service.DeviceAdded += (_, _) => raised++;
            service.DeviceRemoved += (_, _) => raised++;
            var handled = false;

            WithVolumePointer(CreateVolume(0x1, DbtDevTypePort), ptr =>
                service.WndProc(IntPtr.Zero, WmDeviceChange, new IntPtr(eventCode), ptr, ref handled));

            Assert.Equal(0, raised);
        }

        private static DriveDetectionService.DevBroadcastVolume CreateVolume(
            int mask, int deviceType = DbtDevTypeVolume) =>
            new DriveDetectionService.DevBroadcastVolume
            {
                Size = Marshal.SizeOf<DriveDetectionService.DevBroadcastVolume>(),
                DeviceType = deviceType,
                Reserved = 0,
                Mask = mask
            };

        private static void WithVolumePointer(DriveDetectionService.DevBroadcastVolume volume, Action<IntPtr> action)
        {
            var pointer = Marshal.AllocHGlobal(Marshal.SizeOf(volume));
            try
            {
                Marshal.StructureToPtr(volume, pointer, false);
                action(pointer);
            }
            finally
            {
                Marshal.FreeHGlobal(pointer);
            }
        }
    }
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using zDrive.Services;
using zDrive.ViewModels;

namespace zDrive
{
    /// <summary>
    /// MainWindow.xaml code
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new MainViewModel(new RegistryService(), new DriveInfoService());
        }

        #region < Detect Flash Mesage Hook >

        const int WmDevicechange = 0x0219;
        const int DbtDevicearrival = 0x8000;
        const int DbtDeviceremovalcomplete = 0x8004;
        //const int DbtDevtypvolume = 0x00000002;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            if (msg == WmDevicechange)
            {
                var eventCode = wParam.ToInt32();
                bool? isRemove = null;
                switch (eventCode)
                {
                    case DbtDeviceremovalcomplete: // Удалилось устройство
                        isRemove = true;
                        break;
                    case DbtDevicearrival:
                            isRemove = false;
                        break;
                }

                if (isRemove.HasValue)
                {
                    var mainViewModel = DataContext as MainViewModel;
                    mainViewModel?.CheckDisks();
                }

            }
            return IntPtr.Zero;
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
            source?.AddHook(WndProc);
        }

        #endregion

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                popup.IsOpen = false;
                DragMove();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                popup.IsOpen = !popup.IsOpen;
            }
        }

    }
}

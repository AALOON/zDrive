using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using zDrive.Interfaces;
using zDrive.ViewModels;

namespace zDrive
{
    /// <summary>
    /// MainWindow.xaml code
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() => this.InitializeComponent();

        #region < Detect Flash Mesage Hook >

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (PresentationSource.FromVisual(this) is HwndSource source)
            {
                foreach (var wndProc in ViewModelLocator.Ioc.Resolve<IEnumerable<IWndProc>>())
                {
                    source.AddHook(wndProc.WndProc);
                }
            }
        }

        #endregion

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.PropertiesPopup.IsOpen = false;
                this.DragMove();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                this.PropertiesPopup.IsOpen = !this.PropertiesPopup.IsOpen;
            }
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e) => this.PropertiesPopup.IsOpen = false;
    }
}

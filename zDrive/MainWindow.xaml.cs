using System;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        #region < Detect Flash Mesage Hook >
        
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if(PresentationSource.FromVisual(this) is HwndSource source && DataContext is IMainViewModel context)
                source.AddHook(context.WndProc);
        }

        #endregion

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                PropertiesPopup.IsOpen = false;
                DragMove();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                PropertiesPopup.IsOpen = !PropertiesPopup.IsOpen;
            }
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            PropertiesPopup.IsOpen = false;
        }
    }
}

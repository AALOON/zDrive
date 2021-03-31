using System.Windows;
using System.Windows.Input;

namespace zDrive.Mvvm
{
    /// <summary>
    /// Class for DependencyProperty for handle Mouse events
    /// </summary>
    public sealed class MouseBehaviour
    {
        #region < MouseUp >

        public static readonly DependencyProperty MouseUpCommandProperty =
            DependencyProperty.RegisterAttached("MouseUpCommand", typeof(ICommand),
                typeof(MouseBehaviour), new FrameworkPropertyMetadata(MouseUpCommandChanged));

        private static void MouseUpCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            element.MouseUp += element_MouseUp;
        }

        private static void element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;

            var command = GetMouseUpCommand(element);

            command.Execute(e);
        }

        internal static void SetMouseUpCommand(UIElement element, ICommand value) =>
            element.SetValue(MouseUpCommandProperty, value);

        internal static ICommand GetMouseUpCommand(UIElement element) =>
            (ICommand)element.GetValue(MouseUpCommandProperty);

        #endregion

        #region < MouseDown >

        public static readonly DependencyProperty MouseDownCommandProperty =
            DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand),
                typeof(MouseBehaviour), new FrameworkPropertyMetadata(MouseDownCommandChanged));

        private static void MouseDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            element.PreviewMouseDown += ElementMouseDown;
        }

        private static void ElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;

            var command = GetMouseDownCommand(element);

            command.Execute(e);
        }

        internal static void SetMouseDownCommand(UIElement element, ICommand value) =>
            element.SetValue(MouseDownCommandProperty, value);

        internal static ICommand GetMouseDownCommand(UIElement element) =>
            (ICommand)element.GetValue(MouseDownCommandProperty);

        #endregion

        #region < LeftMouseDown >

        public static readonly DependencyProperty LeftMouseButtonDownCommandProperty =
            DependencyProperty.RegisterAttached("LeftMouseButtonDownCommand", typeof(ICommand),
                typeof(MouseBehaviour), new FrameworkPropertyMetadata(LeftMouseButtonDownCommandChanged));

        private static void LeftMouseButtonDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            element.PreviewMouseDown += ElementLeftMouseButtonDown;
        }

        private static void ElementLeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            var element = (FrameworkElement)sender;

            var command = GetLeftMouseButtonDownCommand(element);

            command.Execute(e);
        }

        internal static void SetLeftMouseButtonDownCommand(UIElement element, ICommand value) =>
            element.SetValue(LeftMouseButtonDownCommandProperty, value);

        internal static ICommand GetLeftMouseButtonDownCommand(UIElement element) =>
            (ICommand)element.GetValue(LeftMouseButtonDownCommandProperty);

        #endregion

        #region < RightMouseDown >

        public static readonly DependencyProperty RightMouseButtonDownCommandProperty =
            DependencyProperty.RegisterAttached("RightMouseButtonDownCommand", typeof(ICommand),
                typeof(MouseBehaviour), new FrameworkPropertyMetadata(RightMouseButtonDownCommandChanged));

        private static void RightMouseButtonDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            element.PreviewMouseDown += ElementRightMouseButtonDown;
        }

        private static void ElementRightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Right)
            {
                return;
            }

            var element = (FrameworkElement)sender;

            var command = GetRightMouseButtonDownCommand(element);

            command.Execute(e);
        }

        internal static void SetRightMouseButtonDownCommand(UIElement element, ICommand value) =>
            element.SetValue(RightMouseButtonDownCommandProperty, value);

        internal static ICommand GetRightMouseButtonDownCommand(UIElement element) =>
            (ICommand)element.GetValue(RightMouseButtonDownCommandProperty);

        #endregion
    }
}

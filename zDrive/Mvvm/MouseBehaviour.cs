﻿using System.Windows;
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

            element.PreviewMouseDown += element_MouseDown;
        }

        private static void element_MouseDown(object sender, MouseButtonEventArgs e)
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
    }
}

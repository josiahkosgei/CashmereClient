using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace CashmereDeposit.UserControls
{
    internal class TimeoutDialogBox
    {
        private static Window CreateAutoCloseWindow(TimeSpan timeout)
        {
            Window window1 = new Window
            {
                WindowStyle = WindowStyle.None,
                WindowState = WindowState.Maximized,
                Background = (Brush)Brushes.White,
                AllowsTransparency = true,
                Opacity = 0.5,
                ShowInTaskbar = false,
                ShowActivated = true,
                Topmost = true
            };
            Window window2 = window1;
            window2.Show();
            IntPtr handle = new WindowInteropHelper(window2).Handle;
            Task.Delay((int)timeout.TotalMilliseconds).ContinueWith<IntPtr>((Func<Task, IntPtr>)(t => NativeMethods.SendMessage(handle, 16U, IntPtr.Zero, IntPtr.Zero)));
            return window2;
        }

        public static MessageBoxResult ShowDialog(
          string message,
          int timeout,
          string title = null,
          MessageBoxButton messageBoxButton = MessageBoxButton.OK,
          MessageBoxImage messageBoxImage = MessageBoxImage.None,
          MessageBoxResult defaultMessageBoxResult = MessageBoxResult.No)
        {
            if (timeout <= 0)
                return MessageBox.Show(message, title, messageBoxButton);
            Window autoCloseWindow = CreateAutoCloseWindow(TimeSpan.FromSeconds((double)timeout));
            try
            {
                return MessageBox.Show(autoCloseWindow, message, title, messageBoxButton, messageBoxImage, defaultMessageBoxResult);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                autoCloseWindow.Close();
            }
        }
    }
}

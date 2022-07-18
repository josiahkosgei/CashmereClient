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
          Background = Brushes.White,
          AllowsTransparency = true,
          Opacity = 0.5,
          ShowInTaskbar = false,
          ShowActivated = true,
          Topmost = true
      };

      window1.Show();
      IntPtr handle = new WindowInteropHelper(window1).Handle;
      Task.Delay((int) timeout.TotalMilliseconds).ContinueWith(t => NativeMethods.SendMessage(handle, 16U, IntPtr.Zero, IntPtr.Zero));
      return window1;
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
      Window autoCloseWindow = CreateAutoCloseWindow(TimeSpan.FromSeconds(timeout));
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

using System;
using System.Runtime.InteropServices;

namespace CashmereDeposit.UserControls
{
  internal static class NativeMethods
  {
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(
      IntPtr hWnd,
      uint Msg,
      IntPtr wParam,
      IntPtr lParam);
  }
}

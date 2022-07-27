
// Type: CashmereDeposit.Utils.UtilExtentionMethods




using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CashmereDeposit.Utils
{
    public class UtilExtentionMethods
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        yield return (T)child;
                    foreach (T visualChild in FindVisualChildren<T>(child))
                        yield return visualChild;
                    child = null;
                }
            }
        }

        public static class PasswordBoxHelper
        {
            public static readonly DependencyProperty BoundPasswordProperty = DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnBoundPasswordChanged)));

            public static string GetBoundPassword(DependencyObject d)
            {
                if (d is PasswordBox passwordBox)
                {
                    passwordBox.PasswordChanged -= new RoutedEventHandler(PasswordChanged);
                    passwordBox.PasswordChanged += new RoutedEventHandler(PasswordChanged);
                }
                return (string)d.GetValue(BoundPasswordProperty);
            }

            public static void SetBoundPassword(DependencyObject d, string value)
            {
                if (string.Equals(value, GetBoundPassword(d)))
                    return;
                d.SetValue(BoundPasswordProperty, value);
            }

            private static void OnBoundPasswordChanged(
              DependencyObject d,
              DependencyPropertyChangedEventArgs e)
            {
                if (!(d is PasswordBox passwordBox))
                    return;
                passwordBox.Password = GetBoundPassword(d);
            }

            private static void PasswordChanged(object sender, RoutedEventArgs e)
            {
                PasswordBox passwordBox = sender as PasswordBox;
                SetBoundPassword(passwordBox, passwordBox.Password);
                passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[2]
                {
          passwordBox.Password.Length,
          0
                });
            }
        }
    }
}

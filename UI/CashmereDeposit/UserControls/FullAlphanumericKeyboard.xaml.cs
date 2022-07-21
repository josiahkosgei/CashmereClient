using CashmereDeposit.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace CashmereDeposit.UserControls
{
    public partial class FullAlphanumericKeyboard : UserControl, IComponentConnector
    {
        private bool _capsLockDown = false;
        public bool CapsLockDown
        {
            get => _capsLockDown;
            set
            {
                _capsLockDown = value;
                RenderButtonText();
            }
        }

        public bool ShiftDown { get; set; } = false;

        public FullAlphanumericKeyboard() => InitializeComponent();

        private void SetSelection(PasswordBox passwordBox, int start, int length) => passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke((object)passwordBox, new object[2]
        {
      (object) start,
      (object) length
        });

        private void pressKey(string s)
        {
            s = CapsLockDown ? s.ToUpperInvariant() : s.ToLowerInvariant();
            if (DataContext.GetType() == typeof(PasswordBox))
            {
                PasswordBox dataContext = DataContext as PasswordBox;
                SetSelection(dataContext, dataContext.Password.Length, 0);
                dataContext.Password += s;
                dataContext.Focus();
            }
            else
            {
                TextBox dataContext = DataContext as TextBox;
                int caretIndex = dataContext.CaretIndex;
                if (dataContext.SelectedText.Length > 0)
                    deleteText();
                dataContext.Text = dataContext.Text.Insert(dataContext.CaretIndex, s);
                int num;
                dataContext.CaretIndex = num = caretIndex + 1;
                dataContext.Focus();
            }
        }

        private void Key_Pressed(object sender, RoutedEventArgs e) => pressKey((e.OriginalSource as Button).Content as string);

        private void deleteText()
        {
            if (DataContext.GetType() == typeof(PasswordBox))
            {
                PasswordBox dataContext = DataContext as PasswordBox;
                SetSelection(dataContext, dataContext.Password.Length, 0);
                if (dataContext.Password.Length > 0)
                    dataContext.Password = dataContext.Password.Substring(0, dataContext.Password.Length - 1);
                dataContext.Focus();
            }
            else
            {
                TextBox dataContext = DataContext as TextBox;
                int startIndex = dataContext.CaretIndex;
                if (dataContext.SelectedText.Length > 0)
                {
                    dataContext.Text = dataContext.Text.Remove(dataContext.SelectionStart, dataContext.SelectionLength);
                }
                else
                {
                    startIndex = Math.Max(dataContext.CaretIndex - 1, 0);
                    if (dataContext.CaretIndex > 0)
                        dataContext.Text = dataContext.Text.Remove(startIndex, 1);
                }
                dataContext.CaretIndex = startIndex;
                dataContext.Focus();
            }
        }

        private void keydelete_Click(object sender, RoutedEventArgs e) => deleteText();

        private void key_space_Click(object sender, RoutedEventArgs e) => pressKey(" ");

        private void keyCaps_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is ToggleButton toggleButton))
                return;
            CapsLockDown = toggleButton.IsChecked.HasValue && toggleButton.IsChecked.Value;
        }

        private void keyShift_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
                return;
            ShiftDown = !ShiftDown;
            if (ShiftDown)
                button.BorderThickness = new Thickness(3.0);
            else
                button.BorderThickness = new Thickness(0.0);
        }

        private void RenderButtonText()
        {
            foreach (Button visualChild in UtilExtentionMethods.FindVisualChildren<Button>((DependencyObject)this))
            {
                string content = visualChild.Content as string;
                if (content.Length == 1)
                    visualChild.Content = CapsLockDown ? (object)content.ToUpperInvariant() : (object)content.ToLowerInvariant();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) => RenderButtonText();
    }
}

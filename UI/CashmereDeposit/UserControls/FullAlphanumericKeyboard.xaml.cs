
// Type: CashmereDeposit.UserControls.FullAlphanumericKeyboard

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using CashmereDeposit.Utils;

namespace CashmereDeposit.UserControls
{
  public partial class FullAlphanumericKeyboard : UserControl, IComponentConnector
  {
    private bool _capsLockDown;
    public bool CapsLockDown
    {
      get => _capsLockDown;
      set
      {
        _capsLockDown = value;
        RenderButtonText();
      }
    }

    public bool ShiftDown { get; set; }
        
    private void SetSelection(PasswordBox passwordBox, int start, int length) => passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[2]
    {
      start,
      length
    });

    private void pressKey(string s)
    {
      s = CapsLockDown ? s.ToUpperInvariant() : s.ToLowerInvariant();
      if (DataContext.GetType() == typeof (PasswordBox))
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
      if (DataContext.GetType() == typeof (PasswordBox))
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
      bool? isChecked = toggleButton.IsChecked;
      int num;
      if (!isChecked.HasValue)
      {
        num = 0;
      }
      else
      {
        isChecked = toggleButton.IsChecked;
        num = isChecked.Value ? 1 : 0;
      }
      CapsLockDown = num != 0;
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
      foreach (Button visualChild in UtilExtentionMethods.FindVisualChildren<Button>(this))
      {
        string content = visualChild.Content as string;
        if (content.Length == 1)
          visualChild.Content = CapsLockDown ? content.ToUpperInvariant() : (object) content.ToLowerInvariant();
      }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) => RenderButtonText();

  }
}

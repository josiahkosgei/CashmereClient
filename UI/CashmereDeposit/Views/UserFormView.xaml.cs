using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace CashmereDeposit.Views
{
  public partial class UserFormView : UserControl, IComponentConnector
  {

    public UserFormView()
    {
      InitializeComponent();
      new Style().Setters.Add((SetterBase) new Setter(VisibilityProperty, (object) Visibility.Collapsed));
    }

    private void FieldsListbox_PreviewMouseUp(object sender, MouseButtonEventArgs e) => tabControl.SelectedIndex = FieldsListbox.SelectedIndex + 1;
  }
}

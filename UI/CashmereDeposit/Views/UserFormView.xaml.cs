
// Type: CashmereDeposit.Views.UserFormView

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


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
      new Style().Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
    }

    private void FieldsListbox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        tabControl.SelectedIndex = FieldsListbox.SelectedIndex + 1;
    }
  }
}

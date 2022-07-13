
// Type: CashmereDeposit.ViewModels.ICustomerComboBoxInputScreen

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.Collections.ObjectModel;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
  public interface ICustomerComboBoxInputScreen
  {
    ObservableCollection<string> CustomerComboBoxInput { get; set; }

    string SelectedCustomerComboBoxInput { get; set; }

    GuiScreenListScreen GuiScreenListScreens { get; set; }

    GUIPrepopList GUIPrepopList { get; set; }

    bool AllowFreeText { get; set; }

    bool ComboBoxGridIsVisible { get; set; }

    bool TextBoxGridIsVisible { get; set; }

    bool KeyboardGridIsVisible { get; set; }

    bool ComboBoxButtonsIsVisible { get; set; }

    bool EditComboBoxIsVisible { get; set; }

    bool CancelEditComboBoxIsVisible { get; set; }

    bool IsComboBoxEditMode { get; set; }

    string EditComboBoxButtonCaption { get; set; }

    string CancelEditComboBoxButtonCaption { get; set; }

    bool CanEditComboBox { get; }

    void EditComboBox();

    bool CanCancelEditComboBox { get; }

    void CancelEditComboBox();
  }
}

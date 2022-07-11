
// Type: CashmereDeposit.ViewModels.UserForm.UsernameFormViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Caliburn.Micro;
using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels.UserForm
{
  internal class UsernameFormViewModel : Conductor<Screen>, IShell
  {
    private string _textboxText;
    private string _errorMessageTextBlock;

    public UserControlViewModel UserControlViewModel { get; set; }

    public string DataEntryLabel { get; set; }

    public string DataEntryTextbox
    {
        get { return _textboxText; }
        set
      {
        _textboxText = value;
        NotifyOfPropertyChange("TextBoxText");
      }
    }

    public string ErrorMessageTextBlock
    {
        get { return _errorMessageTextBlock; }
        set
      {
        _errorMessageTextBlock = value;
        NotifyOfPropertyChange(nameof (ErrorMessageTextBlock));
      }
    }

    public UsernameFormViewModel(UserControlViewModel userControlViewModel)
    {
        UserControlViewModel = userControlViewModel;
    }

    public void Validate()
    {
      string str = UserControlViewModel.ValidateUsername(DataEntryTextbox);
      if (str == null)
        TryCloseAsync(new bool?(true));
      else
        ErrorMessageTextBlock = str;
    }

    public void Back()
    {
        TryCloseAsync(new bool?(false));
    }
  }
}

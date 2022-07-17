
//.UserFormViewModel




using Cashmere.Library.CashmereDataAccess.Entities;
using CashmereDeposit.Utils;

namespace CashmereDeposit.ViewModels
{
  internal class UserFormViewModel : FormBase
  {
    public ApplicationUser _applicationUser;
    public ApplicationViewModel _applicationViewModel;
    private string _username;
    public string _password;
    public string _secondPassword;
    public string _firstName;
    public string _lastName;
    public string _email;

    public ApplicationUser ApplicationUser
    {
        get => _applicationUser;
        set
      {
        _applicationUser = value;
        NotifyOfPropertyChange(nameof (ApplicationUser));
      }
    }

    public ApplicationViewModel ApplicationViewModel => _applicationViewModel;

    public string Username
    {
        get => _username;
        set
      {
        _username = value;
        NotifyOfPropertyChange(nameof (Username));
      }
    }

    public string UsernameError { get; set; } = "Default error";

    public string Password
    {
        get => _password;
        set
      {
        _password = value;
        NotifyOfPropertyChange(nameof (Password));
      }
    }

    public string SecondPassword
    {
        get => _secondPassword;
        set
      {
        _secondPassword = value;
        NotifyOfPropertyChange(nameof (SecondPassword));
      }
    }

    public string FirstName
    {
        get => _firstName;
        set
      {
        _firstName = value;
        NotifyOfPropertyChange(nameof (FirstName));
      }
    }

    public string LastName
    {
        get => _lastName;
        set
      {
        _lastName = value;
        NotifyOfPropertyChange(nameof (LastName));
      }
    }

    public string Email
    {
        get => _email;
        set
      {
        _email = value;
        NotifyOfPropertyChange(nameof (Email));
      }
    }

    public UserFormViewModel(
      ApplicationViewModel applicationViewModel,
      ApplicationUser applicationUser = null)
    {
      if (applicationUser == null)
      {
        ApplicationUser = new ApplicationUser();
      }
      else
      {
        ApplicationUser = applicationUser;
        Username = "Hello";
        FirstName = _applicationUser?.Fname;
        LastName = _applicationUser?.Lname;
        Email = _applicationUser?.Email;
      }
      _applicationViewModel = applicationViewModel;
      Username = "JDOE";
      FirstName = "JOHN";
      LastName = "DOE";
      Email = "JDOE@EXAMPLEBANK.COM";
    }

    public override void CloseFormField()
    {
    }

    public void UsernameBack()
    {
        Username = ApplicationUser.Username;
    }

    public void ValidateUsername()
    {
      if (string.IsNullOrWhiteSpace(Username))
        UsernameError = "Please enter a username";
      else
        ApplicationUser.Username = Username;
    }
  }
}

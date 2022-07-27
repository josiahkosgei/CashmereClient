using Caliburn.Micro;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace CashmereDeposit.ViewModels
{
    internal class UserControlViewModel : FormViewModelBase
    {
        //  private readonly DepositorDBContext _depositorDBContext;

        private ApplicationUser ApplicationUser;
        private string _username;
        public string _password;
        public string _passwordHash;
        public string _secondPassword;
        public string _firstName;
        public string _lastName;
        public string _email;
        public Role _role;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyOfPropertyChange(nameof(Username));
            }
        }

        public string UsernameError { get; set; } = "Default error";

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(nameof(Password));
            }
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                _passwordHash = value;
                NotifyOfPropertyChange(nameof(PasswordHash));
            }
        }

        public string SecondPassword
        {
            get => _secondPassword;
            set
            {
                _secondPassword = value;
                NotifyOfPropertyChange(nameof(SecondPassword));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(nameof(LastName));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                NotifyOfPropertyChange(nameof(Email));
            }
        }

        public Role Role
        {
            get => _role;
            set
            {
                _role = value;
                NotifyOfPropertyChange(nameof(Role));
            }
        }
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IRoleRepository _roleRepository;

        public UserControlViewModel(
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject,
          ApplicationUser applicationUser,
          bool isNewEntry)
          : base(applicationViewModel, conductor, callingObject, isNewEntry)
        {
            _applicationUserRepository = IoC.Get<IApplicationUserRepository>();
            _roleRepository = IoC.Get<IRoleRepository>();
            ApplicationUser = applicationUser;
            Username = ApplicationUser?.Username;
            FirstName = ApplicationUser?.Fname;
            LastName = ApplicationUser?.Lname;
            Email = ApplicationUser?.Email;
            Role = ApplicationUser?.Role;
            PasswordHash = applicationUser?.Password;
            var list = _roleRepository.GetAllAsync().Select(x => x.Name).ToList();
            Fields.Add(new FormListItem()
            {
                DataLabel = nameof(Username),
                Validate = new Func<string, string>(ValidateUsername),
                ValidatedText = Username,
                DataTextBoxLabel = Username,
                FormListItemType = FormListItemType.ALPHATEXTBOX
            });
            Fields.Add(new FormListItem()
            {
                DataLabel = nameof(Password),
                Validate = new Func<string, string>(ValidatePassword),
                ValidatedText = "********",
                FormListItemType = FormListItemType.ALPHAPASSWORD
            });
            Fields.Add(new FormListItem()
            {
                DataLabel = "Re-Enter Password",
                Validate = new Func<string, string>(ValidateSecondPassword),
                ValidatedText = "********",
                FormListItemType = FormListItemType.ALPHAPASSWORD
            });
            Fields.Add(new FormListItem()
            {
                DataLabel = "First Name",
                Validate = new Func<string, string>(ValidateFirstName),
                ValidatedText = FirstName,
                DataTextBoxLabel = FirstName,
                FormListItemType = FormListItemType.ALPHATEXTBOX
            });
            Fields.Add(new FormListItem()
            {
                DataLabel = "Last Name",
                Validate = new Func<string, string>(ValidateLastName),
                ValidatedText = LastName,
                DataTextBoxLabel = LastName,
                FormListItemType = FormListItemType.ALPHATEXTBOX
            });
            Fields.Add(new FormListItem()
            {
                DataLabel = nameof(Email),
                Validate = new Func<string, string>(ValidateEmail),
                ValidatedText = Email,
                DataTextBoxLabel = Email,
                FormListItemType = FormListItemType.ALPHATEXTBOX
            });
            Fields.Add(new FormListItem()
            {
                DataLabel = nameof(Role),
                ItemList = list,
                Validate = new Func<string, string>(ValidateRole),
                ValidatedText = Role?.Name,
                DataTextBoxLabel = Role?.Name,
                FormListItemType = FormListItemType.LISTBOX
            });
            if (isNew)
                ScreenTitle = "Create New User";
            ActivateItemAsync(new FormListViewModel(this));
        }

        public string ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Please enter a username";
            if (isNew)
            {
                if (_applicationUserRepository.GetAllAsync().Any(x => x.Username.ToUpper() == username.ToUpper()))
                    return "User already exists";
            }
            return null;
        }

        public string ValidatePassword(string password)
        {
            return null;
        }

        public string ValidateSecondPassword(string password)
        {
            if (!(password == Password))
                return "Passwords do not match";
            SecondPassword = password;
            return null;
        }

        public string ValidateFirstName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Please enter a first name";
            FirstName = name;
            return null;
        }

        public string ValidateLastName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Please enter a last name";
            LastName = name;
            return null;
        }

        private string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Please enter an email";
            if (!email.isEmail())
                return "Invalid email address entered";
            Email = email;
            return null;
        }

        private string ValidateRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return "Please select a role";
            var role1 = _roleRepository.GetByNameAsync(role);
            if (role1 == null)
                return "Role does not exist";
            Role = role1;
            return null;
        }

        public override async Task<string> SaveForm()
        {
            string str = null;
            if (FormValidation() > 0)
                return "Form validation failed with {0} errors. Kindly correct them and try saving again";
            ApplicationUser.Username = Username;
            ApplicationUser.Password = PasswordHash;
            ApplicationUser.Fname = FirstName;
            ApplicationUser.Lname = LastName;
            ApplicationUser.Email = Email;
            ApplicationUser.RoleId = Role.Id;
            if (isNew)

                try
                {
                     _applicationUserRepository.AddAsync(ApplicationUser);
                }
                catch (Exception ex)
                {
                    ApplicationViewModel.Log.Error(GetType().Name, nameof(UserControlViewModel), nameof(SaveForm), "Error saving user to database: {0}", new object[1]
                    {
          ex.MessageString()
                    });
                    return "Error occurred while creating user.";
                }
            return str;
        }

        public override int FormValidation()
        {
            var num = 0;
            foreach (var field in Fields)
            {
                var validate = field.Validate;
                var str = validate != null ? validate((field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE ? field.DataTextBoxLabel : field.ValidatedText) : null;
                if (str != null)
                {
                    field.ErrorMessageTextBlock = str;
                    ++num;
                }
            }
            return num;
        }
    }
}

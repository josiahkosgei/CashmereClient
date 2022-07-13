
// Type: CashmereDeposit.ViewModels.UserLoginViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Caliburn.Micro;
using Cashmere.API.Messaging.Authentication;
using Cashmere.API.Messaging.Authentication.Clients;
using Cashmere.Library.Standard.Security;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;

using CashmereDeposit.Utils;
using CashmereDeposit.Utils.AlertClasses;

namespace CashmereDeposit.ViewModels
{
    [Guid("34E3150D-E6AE-4F87-807C-9AC9C57DE6B0")]
    public class UserLoginViewModel : FormViewModelBase
    {
        private string _username;
        public string _password;
        public string Activity;
        public bool IsAuthorise;
        public PermissionRequiredResult _permissionRequiredResult = new PermissionRequiredResult()
        {
            LoginSuccessful = false,
            ApplicationUser = null
        };

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(nameof(Username));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(nameof(Password));
            }
        }

        public ApplicationUser ApplicationUser { get; set; }

        public object NextObject { get; set; }

        public PermissionRequiredResult PermissionRequiredResult
        {
            get { return _permissionRequiredResult; }
        }

        private bool SplitAuthorise { get; set; }

        private LoginSuccessCallBack LoginSuccessCallBackDelegate { get; set; }

        public UserLoginViewModel(
          ApplicationViewModel applicationViewModel,
          Conductor<Screen> conductor,
          Screen callingObject,
          object nextObject,
          string activity,
          bool isAuthorise = false,
          bool splitAuthorise = false,
          LoginSuccessCallBack loginSuccessCallBack = null)
          : base(applicationViewModel, conductor, callingObject, false)
        {
            Activity = activity;
            ScreenTitle = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor ScreenTitle", "sys_LoginUserScreenTitle", "Login");
            IsAuthorise = isAuthorise;
            ApplicationViewModel = applicationViewModel;
            NextObject = nextObject;
            SplitAuthorise = splitAuthorise;
            LoginSuccessCallBackDelegate = loginSuccessCallBack;
            Fields.Add(new FormListItem()
            {
                DataLabel = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("UserLoginViewModel.contructor", "sys_UsernameLabel_Caption", nameof(Username)),
                Validate = new Func<string, string>(ValidateUsername),
                DataTextBoxLabel = Username,
                FormListItemType = FormListItemType.ALPHATEXTBOX
            });
            Fields.Add(new FormListItem()
            {
                DataLabel = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("UserLoginViewModel.contructor", "sys_PasswordLabel_Caption", nameof(Password)),
                Validate = new Func<string, string>(ValidatePassword),
                FormListItemType = FormListItemType.ALPHAPASSWORD
            });
            ActivateItemAsync(new FormListViewModel(this)
            {
                NextCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("UserLoginViewModel.contructor", "\tsys_Login_NextCaption", "Login"),
                BackCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("UserLoginViewModel.contructor", "sys_Login_BackCaption", "Cancel")
            });
        }

        public string ValidateUsername(string username)
        {
            if (username == null)
                return "Enter a username";
            Username = username;
            return null;
        }

        public string ValidatePassword(string password)
        {
            if (password == null)
                return "Enter a password";
            Password = password;
            return null;
        }

        public override async Task<string> SaveForm()
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            try
            {
                Device device = ApplicationViewModel.ApplicationModel.GetDevice(DBContext);
                ApplicationViewModel.Log.Debug(GetType().Name, nameof(SaveForm), "Form", "Saving form");
                FormErrorText = "";
                int num = await FormValidation();
                ApplicationViewModel.SaveToDatabase(DBContext);
                switch (num)
                {
                    case 0:
                        ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(SaveForm), "Form", "login form validation successful for {0}", Username);
                        device.LoginAttempts = 0;
                        ApplicationViewModel.SaveToDatabase(DBContext);
                        return null;
                    case 9:
                        NextObject = new UserChangePasswordFormViewModel(ApplicationViewModel, ApplicationUser, null, Conductor, CallingObject, NextObject, IsAuthorise, LoginSuccessCallBackDelegate);
                        goto case 0;
                    case 10:
                        ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(SaveForm), "Form", "Auth Server connection fault: user = {0}", Username);
                        ApplicationViewModel.SaveToDatabase(DBContext);
                        break;
                    default:
                        ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(SaveForm), "Form", "loginform validation failed for {0}, incrementing loginfails", Username);
                        ++device.LoginAttempts;
                        if (device.Enabled && ApplicationViewModel.DeviceConfiguration.LOGINFAIL_DEVICELOCK && device.LoginAttempts >= ApplicationViewModel.DeviceConfiguration.LOGINFAIL_DEVICELOCK_RETRY_COUNT * ApplicationViewModel.DeviceConfiguration.LOGINFAIL_MAX_CYCLES)
                            ApplicationViewModel.LockDevice(true, ApplicationErrorConst.ERROR_LOGIN, "Maximum device login attempts reached " + (ApplicationViewModel.DeviceConfiguration.LOGINFAIL_DEVICELOCK_RETRY_COUNT * ApplicationViewModel.DeviceConfiguration.LOGINFAIL_MAX_CYCLES).ToString());
                        ApplicationViewModel.SaveToDatabase(DBContext);
                        break;
                }
            }
            catch (TimeoutException ex)
            {
                ApplicationViewModel.Log.ErrorFormat(GetType().Name, 114, ApplicationErrorConst.ERROR_TIMEOUT.ToString(), "Login Error: {0}", ex.MessageString());
                FormErrorText = "Login unavailable. Please try again later or contact Administrator";
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.ErrorFormat(GetType().Name, 1, ApplicationErrorConst.ERROR_GENERAL.ToString(), "Login Error: {0}", ex.MessageString());
                FormErrorText = "Login Error. Contact Administrator";
            }
            ApplicationViewModel.SaveToDatabase(DBContext);
            return FormErrorText;
        }

        public new async Task<int> FormValidation()
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Device device = ApplicationViewModel.ApplicationModel.GetDevice(DBContext);
            foreach (FormListItem field in Fields)
            {
                Func<string, string> validate = field.Validate;
                string str = validate != null ? validate((field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE ? field.DataTextBoxLabel : field.ValidatedText) : null;
                if (str != null)
                {
                    field.ErrorMessageTextBlock = str;
                    FormErrorText = "Please enter a username and password";
                    ApplicationViewModel.Log.Warning(GetType().Name, "Login Denied", "Login", FormErrorText);
                    ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, FormErrorText, Username));
                    return 4;
                }
            }
            var depositorContextProcedures = new DepositorDBContextProcedures(depositorDbContext);
            ApplicationUser dbApplicationUser = depositorDbContext.Devices.Where(de => de.UserGroup == device.UserGroup).Select(dv =>
                dv.UserGroupNavigation.ApplicationUsers.FirstOrDefault(x =>!(bool)x.UserDeleted && (bool)x.Username.Equals(Username, StringComparison.InvariantCultureIgnoreCase))).FirstOrDefault();

            //var dbApplicationUser = depositorContextProcedures
            //    .GetDeviceUsersByDeviceAsync(device.UserGroup).Result
            //    .FirstOrDefault(x =>
            //        !(bool)x.UserDeleted &&
            //        (bool)x.username.Equals(Username, StringComparison.InvariantCultureIgnoreCase));
            int num1;
            if (dbApplicationUser == null)
            {
                FormErrorText = "Username or password is incorrect.";
                ApplicationViewModel.Log.Warning(GetType().Name, "Login Denied", "Login", FormErrorText);
                ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, FormErrorText, Username, dbApplicationUser));
                num1 = 1;
            }
            else
            {
                DeviceLogin entity = new DeviceLogin()
                {
                    Id = Guid.NewGuid(),
                    User = dbApplicationUser.Id,
                    LoginDate = DateTime.Now,
                    DeviceId = device.Id
                };
                DeviceLogin deviceLogin1 = (await DBContext.DeviceLogins.AddAsync(entity)).Entity;

                var applicationUser1 = dbApplicationUser;
                int num2;
                if (applicationUser1 == null)
                {
                    num2 = 1;
                }
                else
                {
                    bool? depositorEnabled = applicationUser1.DepositorEnabled;
                    bool flag = true;
                    num2 = !(depositorEnabled.GetValueOrDefault() == flag & depositorEnabled.HasValue) ? 1 : 0;
                }
                if (num2 != 0)
                {
                    FormErrorText = "User has been locked";
                    ApplicationViewModel.Log.Warning(GetType().Name, "Login Denied", "Login", FormErrorText);
                    ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, FormErrorText, Username, dbApplicationUser));
                    num1 = 3;
                }
                else
                {
                    Guid id1 = dbApplicationUser.Id;
                    Guid? id2 = ApplicationViewModel.CurrentUser?.Id;
                    if ((id2.HasValue ? (id1 == id2.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                    {
                        FormErrorText = "Permission Denied";
                        string errorMessage = string.Format("User {0} cannot initialise and authenticate permission {1}. Dual custody required. Permission Denied", dbApplicationUser.Username, "BACKEND_MENU_SHOW");
                        ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, errorMessage, Username, dbApplicationUser));
                        num1 = 2;
                    }
                    else
                    {
                        AuthenticationResponse result = Task.Run((Func<Task<AuthenticationResponse>>)(() => AuthenticationAsync(dbApplicationUser, Password))).Result;
                        if (!result.IsSuccess)
                        {
                            if (result.IsInvalidCredentials)
                            {
                                ++dbApplicationUser.LoginAttempts;
                                if (dbApplicationUser.LoginAttempts >= ApplicationViewModel.DeviceConfiguration.LOGINFAIL_DEVICELOCK_RETRY_COUNT)
                                {
                                    FormErrorText = "User has been locked";
                                    ApplicationViewModel.LockUser(dbApplicationUser, true, ApplicationErrorConst.WARN_LOCKING_USER, "Login Failure limit has been reached");
                                    dbApplicationUser.LoginAttempts = 0;
                                    ApplicationViewModel.SaveToDatabase(DBContext);
                                    ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, FormErrorText, Username, dbApplicationUser));
                                    num1 = 3;
                                }
                                else
                                {
                                    ApplicationViewModel.SaveToDatabase(DBContext);
                                    FormErrorText = "Username or password is incorrect.";
                                    ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, FormErrorText, Username, dbApplicationUser));
                                    num1 = 1;
                                }
                            }
                            else
                            {
                                ApplicationViewModel.Log.Warning(nameof(UserLoginViewModel), "FAIL", nameof(FormValidation), "Error during login: {0}", new object[1]
                                {
                                    result.ToString()
                                });
                                FormErrorText = "Login Error. Contact administrator.";
                                ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, FormErrorText, Username, dbApplicationUser));
                                num1 = 10;
                            }
                        }
                        else if (!AuthenticationAndAuthorisation.Authenticate(ApplicationViewModel, dbApplicationUser, Activity, IsAuthorise))
                        {
                            FormErrorText = "Permission Denied";
                            string errorMessage = string.Format("User {0} does not have the {1} permission. Permission Denied isAuth={2}", dbApplicationUser.Username, Activity, IsAuthorise);
                            ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, errorMessage, Username, dbApplicationUser));
                            num1 = 2;
                        }
                        else
                        {
                            num1 = 0;
                            deviceLogin1.Success = new bool?(true);
                            string errorMessage = string.Format("User {0} logged in with permission {1} successfully", dbApplicationUser.Username, "BACKEND_MENU_SHOW");
                            dbApplicationUser.LoginAttempts = 0;
                            device.LoginAttempts = 0;
                            device.LoginCycles = 0;
                            ApplicationViewModel.AlertManager.SendAlert(new AlertLoginSuccess(device, DateTime.Now, errorMessage, Username, dbApplicationUser));
                            ApplicationViewModel.SaveToDatabase(DBContext);
                            ApplicationUser = dbApplicationUser;
                            _permissionRequiredResult = new PermissionRequiredResult()
                            {
                                LoginSuccessful = true,
                                ApplicationUser = ApplicationUser
                            };
                            if (!dbApplicationUser.IsAdUser)
                            {
                                ApplicationUser applicationUser2 = dbApplicationUser;
                                if ((applicationUser2 != null ? (applicationUser2.PasswordResetRequired ? 1 : 0) : 0) != 0)
                                {
                                    FormErrorText = "Password reset required. please enter a new password";
                                    ApplicationViewModel.Log.Warning(GetType().Name, "Login OK", "Login", "Password reset required.");
                                    num1 = 9;
                                }
                                else
                                {
                                    PasswordPolicy passwordPolicy = DBContext.PasswordPolicies.FirstOrDefault();
                                    if (passwordPolicy != null)
                                    {
                                        ApplicationUser applicationUser3 = dbApplicationUser;
                                        PasswordHistory passwordHistory;
                                        if (applicationUser3 == null)
                                        {
                                            passwordHistory = null;
                                        }
                                        else
                                        {
                                            ICollection<PasswordHistory> passwordHistories = applicationUser3.PasswordHistories;
                                            if (passwordHistories == null)
                                            {
                                                passwordHistory = null;
                                            }
                                            else
                                            {
                                                IEnumerable<PasswordHistory> source = passwordHistories.Where(x => x.Password == dbApplicationUser.Password);
                                                passwordHistory = source != null ? source.FirstOrDefault() : (PasswordHistory)null;
                                            }
                                        }
                                        DateTime? logDate = passwordHistory?.LogDate;
                                        DateTime dateTime1 = DateTime.Now;
                                        DateTime dateTime2 = dateTime1.AddDays(-passwordPolicy.ExpiryDays);
                                        DepositorLogger log = ApplicationViewModel.Log;
                                        string name = GetType().Name;
                                        object[] objArray = new object[3]
                                        {
                                            dateTime2,
                                            logDate,
                                            null
                                        };
                                        dateTime1 = dateTime2;
                                        DateTime? nullable = logDate;
                                        objArray[2] = nullable.HasValue ? (dateTime1 >= nullable.GetValueOrDefault() ? 1 : 0) : 0;
                                        log.DebugFormat(name, "Check Password Expiry", "Login", "checkDate {0} >= passwordCreationDate {1} = {2}", objArray);
                                        if (passwordPolicy != null && passwordPolicy.ExpiryDays > 0 && logDate.HasValue)
                                        {
                                            dateTime1 = dateTime2;
                                            nullable = logDate;
                                            if ((nullable.HasValue ? (dateTime1 >= nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                                            {
                                                FormErrorText = "Password has expired please enter a new password in web app";
                                                ApplicationViewModel.Log.Warning(GetType().Name, "Login Denied", "Login", FormErrorText);
                                                ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, FormErrorText, Username, dbApplicationUser));
                                                num1 = 9;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                deviceLogin1.Message = FormErrorText;
                DeviceLogin deviceLogin2 = deviceLogin1;
                bool? success = deviceLogin1.Success;
                bool? nullable1 = new bool?(success.HasValue ? success.GetValueOrDefault() : string.IsNullOrEmpty(FormErrorText));
                deviceLogin2.Success = nullable1;
                deviceLogin1.DepositorEnabled = dbApplicationUser.DepositorEnabled;
                deviceLogin1.ChangePassword = new bool?(!dbApplicationUser.IsAdUser && dbApplicationUser.PasswordResetRequired);
            }
            ApplicationViewModel.SaveToDatabase(DBContext);
            return num1;
        }

        public override void FormClose(bool success)
        {
            if (success)
            {
                _permissionRequiredResult = new PermissionRequiredResult()
                {
                    LoginSuccessful = true,
                    ApplicationUser = ApplicationUser
                };
                if (IsAuthorise || SplitAuthorise)
                    ApplicationViewModel.ValidatingUser = ApplicationUser;
                else
                    ApplicationViewModel.CurrentUser = ApplicationUser;
                Conductor.ActivateItemAsync(NextObject);
            }
            else
            {
                _permissionRequiredResult = new PermissionRequiredResult()
                {
                    LoginSuccessful = false,
                    ApplicationUser = null
                };
                Conductor.ActivateItemAsync(CallingObject);
            }
            if (NextObject is UserChangePasswordFormViewModel)
                return;
            LoginSuccessCallBack callBackDelegate = LoginSuccessCallBackDelegate;
            if (callBackDelegate == null)
                return;
            callBackDelegate(ApplicationUser, IsAuthorise || SplitAuthorise);
        }

        private bool StandardAuthentication(ApplicationUser user)
        {
            return PasswordStorage.VerifyPassword(Password, user.Password);
        }

        private async Task<AuthenticationResponse> AuthenticationAsync(
          ApplicationUser user,
          string password)
        {
            UserLoginViewModel userLoginViewModel = this;
            using DepositorDBContext depositorDBContext = new DepositorDBContext();
            Device device = userLoginViewModel.ApplicationViewModel.ApplicationModel.GetDevice(depositorDBContext);
            if (user.IsAdUser || !ApplicationViewModel.DeviceConfiguration.ALLOW_OFFLINE_AUTH)
            {
                AuthenticationRequest authenticationRequest = new AuthenticationRequest
                {
                    AppID = device.AppId,
                    AppName = device.MachineName
                };
                Guid guid = Guid.NewGuid();
                authenticationRequest.SessionID = guid.ToString();
                authenticationRequest.Language = userLoginViewModel.ApplicationViewModel.CurrentLanguage;
                guid = Guid.NewGuid();
                authenticationRequest.MessageID = guid.ToString();
                authenticationRequest.IsADUser = user.IsAdUser;
                authenticationRequest.MessageDateTime = DateTime.Now;
                authenticationRequest.Username = user.Username;
                authenticationRequest.Password = password.Encrypt(device.AppKey);
                AuthenticationRequest request = authenticationRequest;
                AuthenticationServiceClient authenticationServiceClient = new AuthenticationServiceClient(ApplicationViewModel.DeviceConfiguration.API_AUTH_API_URI, device.AppId, device.AppKey, null);
                ApplicationViewModel.Log.InfoFormat(nameof(UserLoginViewModel), nameof(AuthenticationAsync), "Request", "Sending request {0}", request.ToString());
                AuthenticationResponse authenticationResponse = await authenticationServiceClient.AuthenticateAsync(request);
                ApplicationViewModel.Log.DebugFormat(nameof(UserLoginViewModel), nameof(AuthenticationAsync), "Response", "Received response {0}", authenticationResponse);
                if (authenticationResponse.IsSuccess)
                    ApplicationViewModel.Log.InfoFormat(nameof(UserLoginViewModel), nameof(AuthenticationAsync), "Login", "Login SUCCESS for request {0}, User {1}", request.MessageID, user.Username);
                else
                    ApplicationViewModel.Log.WarningFormat(nameof(UserLoginViewModel), nameof(AuthenticationAsync), "Login", "Login FAIL for request {0}, User {1}: {2}", request.MessageID, user.Username, authenticationResponse.ServerErrorMessage);
                ApplicationViewModel.SaveToDatabase(depositorDBContext);
                return authenticationResponse;
            }
            bool flag = userLoginViewModel.StandardAuthentication(user);
            AuthenticationResponse authenticationResponse1 = new AuthenticationResponse
            {
                AppID = device.AppId,
                AppName = device.MachineName,
                SessionID = Guid.NewGuid().ToString()
            };
            Guid guid1 = Guid.NewGuid();
            authenticationResponse1.RequestID = guid1.ToString();
            guid1 = Guid.NewGuid();
            authenticationResponse1.MessageID = guid1.ToString();
            authenticationResponse1.MessageDateTime = DateTime.Now;
            authenticationResponse1.IsInvalidCredentials = !flag;
            authenticationResponse1.IsSuccess = flag;
            authenticationResponse1.PublicErrorCode = null;
            authenticationResponse1.PublicErrorMessage = null;
            authenticationResponse1.ServerErrorCode = null;
            authenticationResponse1.ServerErrorMessage = null;
            return authenticationResponse1;
        }

        public delegate void LoginSuccessCallBack(ApplicationUser applicationUser, bool isAuth);
    }
}

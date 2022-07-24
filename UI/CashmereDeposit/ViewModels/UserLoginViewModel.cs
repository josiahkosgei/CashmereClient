
//.UserLoginViewModel




using Caliburn.Micro;
using Cashmere.API.Messaging.Authentication;
using Cashmere.API.Messaging.Authentication.Clients;
using Cashmere.Library.Standard.Security;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
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
        public PermissionRequiredResult _permissionRequiredResult = new()
        {
            LoginSuccessful = false,
            ApplicationUser = null
        };

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyOfPropertyChange(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(nameof(Password));
            }
        }

        public ApplicationUser ApplicationUser { get; set; }

        public object NextObject { get; set; }

        public PermissionRequiredResult PermissionRequiredResult => _permissionRequiredResult;

        private bool SplitAuthorise { get; set; }

        private LoginSuccessCallBack LoginSuccessCallBackDelegate { get; set; }
       //  private static DepositorDBContext _depositorDBContext { get; set; }

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
            _depositorDBContext = IoC.Get<DepositorDBContext>();
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
            try
            {
                var device = ApplicationViewModel.ApplicationModel.GetDeviceAsync();
                ApplicationViewModel.Log.Debug(GetType().Name, nameof(SaveForm), "Form", "Saving form");
                FormErrorText = "";
                var num = await FormValidation();
                _depositorDBContext.SaveChangesAsync().Wait();
                switch (num)
                {
                    case 0:
                        ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(SaveForm), "Form", "login form validation successful for {0}", Username);
                        device.LoginAttempts = 0;
                        _depositorDBContext.SaveChangesAsync().Wait();
                        return null;
                    case 9:
                        NextObject = new UserChangePasswordFormViewModel(ApplicationViewModel, ApplicationUser, null, Conductor, CallingObject, NextObject, IsAuthorise, LoginSuccessCallBackDelegate);
                        goto case 0;
                    case 10:
                        ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(SaveForm), "Form", "Auth Server connection fault: user = {0}", Username);
                        _depositorDBContext.SaveChangesAsync().Wait();
                        break;
                    default:
                        ApplicationViewModel.Log.InfoFormat(GetType().Name, nameof(SaveForm), "Form", "loginform validation failed for {0}, incrementing loginfails", Username);
                        ++device.LoginAttempts;
                        if (device.Enabled && ApplicationViewModel.DeviceConfiguration.LOGINFAIL_DEVICELOCK && device.LoginAttempts >= ApplicationViewModel.DeviceConfiguration.LOGINFAIL_DEVICELOCK_RETRY_COUNT * ApplicationViewModel.DeviceConfiguration.LOGINFAIL_MAX_CYCLES)
                            ApplicationViewModel.LockDevice(true, ApplicationErrorConst.ERROR_LOGIN, "Maximum device login attempts reached " + (ApplicationViewModel.DeviceConfiguration.LOGINFAIL_DEVICELOCK_RETRY_COUNT * ApplicationViewModel.DeviceConfiguration.LOGINFAIL_MAX_CYCLES).ToString());
                        _depositorDBContext.SaveChangesAsync().Wait();
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
            _depositorDBContext.SaveChangesAsync().Wait();
            return FormErrorText;
        }

        public new async Task<int> FormValidation()
        {
            var device = ApplicationViewModel.ApplicationModel.GetDeviceAsync();
            foreach (var field in Fields)
            {
                var validate = field.Validate;
                var str = validate != null ? validate((field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE ? field.DataTextBoxLabel : field.ValidatedText) : null;
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
            var dbApplicationUser = _depositorDBContext.Devices.Where(de => de.UserGroup == device.UserGroup).Select(dv =>
                dv.UserGroupNavigation.ApplicationUsers.FirstOrDefault(x => !(bool)x.UserDeleted && (bool)x.Username.Equals(Username, StringComparison.InvariantCultureIgnoreCase))).FirstOrDefault();

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
                var entity = new DeviceLogin()
                {
                    Id = Guid.NewGuid(),
                    User = dbApplicationUser.Id,
                    LoginDate = DateTime.Now,
                    DeviceId = device.Id
                };
                var deviceLogin1 = (await _depositorDBContext.DeviceLogins.AddAsync(entity)).Entity;

                var applicationUser1 = dbApplicationUser;
                int num2;
                if (applicationUser1 == null)
                {
                    num2 = 1;
                }
                else
                {
                    var depositorEnabled = applicationUser1.DepositorEnabled;
                    var flag = true;
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
                    var id1 = dbApplicationUser.Id;
                    var id2 = ApplicationViewModel.CurrentUser?.Id;
                    if ((id2.HasValue ? id1 == id2.GetValueOrDefault() ? 1 : 0 : 0) != 0)
                    {
                        FormErrorText = "Permission Denied";
                        var errorMessage = string.Format("User {0} cannot initialise and authenticate permission {1}. Dual custody required. Permission Denied", dbApplicationUser.Username, "BACKEND_MENU_SHOW");
                        ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, errorMessage, Username, dbApplicationUser));
                        num1 = 2;
                    }
                    else
                    {
                        var result = Task.Run((Func<Task<AuthenticationResponse>>)(() => AuthenticationAsync(dbApplicationUser, Password))).Result;
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
                                    _depositorDBContext.SaveChangesAsync().Wait();
                                    ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, FormErrorText, Username, dbApplicationUser));
                                    num1 = 3;
                                }
                                else
                                {
                                    _depositorDBContext.SaveChangesAsync().Wait();
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
                            var errorMessage = string.Format("User {0} does not have the {1} permission. Permission Denied isAuth={2}", dbApplicationUser.Username, Activity, IsAuthorise);
                            ApplicationViewModel.AlertManager.SendAlert(new AlertLoginFailed(device, DateTime.Now, errorMessage, Username, dbApplicationUser));
                            num1 = 2;
                        }
                        else
                        {
                            num1 = 0;
                            deviceLogin1.Success = new bool?(true);
                            var errorMessage = string.Format("User {0} logged in with permission {1} successfully", dbApplicationUser.Username, "BACKEND_MENU_SHOW");
                            dbApplicationUser.LoginAttempts = 0;
                            device.LoginAttempts = 0;
                            device.LoginCycles = 0;
                            ApplicationViewModel.AlertManager.SendAlert(new AlertLoginSuccess(device, DateTime.Now, errorMessage, Username, dbApplicationUser));
                            _depositorDBContext.SaveChangesAsync().Wait();
                            ApplicationUser = dbApplicationUser;
                            _permissionRequiredResult = new PermissionRequiredResult()
                            {
                                LoginSuccessful = true,
                                ApplicationUser = ApplicationUser
                            };
                            if (!dbApplicationUser.IsAdUser)
                            {
                                var applicationUser2 = dbApplicationUser;
                                if ((applicationUser2 != null ? applicationUser2.PasswordResetRequired ? 1 : 0 : 0) != 0)
                                {
                                    FormErrorText = "Password reset required. please enter a new password";
                                    ApplicationViewModel.Log.Warning(GetType().Name, "Login OK", "Login", "Password reset required.");
                                    num1 = 9;
                                }
                                else
                                {
                                    var passwordPolicy = _depositorDBContext.PasswordPolicies.FirstOrDefault();
                                    if (passwordPolicy != null)
                                    {
                                        var applicationUser3 = dbApplicationUser;
                                        PasswordHistory passwordHistory;
                                        if (applicationUser3 == null)
                                        {
                                            passwordHistory = null;
                                        }
                                        else
                                        {
                                            var passwordHistories = applicationUser3.PasswordHistories;
                                            if (passwordHistories == null)
                                            {
                                                passwordHistory = null;
                                            }
                                            else
                                            {
                                                var source = passwordHistories.Where(x => x.Password == dbApplicationUser.Password);
                                                passwordHistory = source != null ? source.FirstOrDefault() : (PasswordHistory)null;
                                            }
                                        }
                                        var logDate = passwordHistory?.LogDate;
                                        var dateTime1 = DateTime.Now;
                                        var dateTime2 = dateTime1.AddDays(-passwordPolicy.ExpiryDays);
                                        var log = ApplicationViewModel.Log;
                                        var name = GetType().Name;
                                        var objArray = new object[3]
                                        {
                                            dateTime2,
                                            logDate,
                                            null
                                        };
                                        dateTime1 = dateTime2;
                                        var nullable = logDate;
                                        objArray[2] = nullable.HasValue ? dateTime1 >= nullable.GetValueOrDefault() ? 1 : 0 : 0;
                                        log.DebugFormat(name, "Check Password Expiry", "Login", "checkDate {0} >= passwordCreationDate {1} = {2}", objArray);
                                        if (passwordPolicy != null && passwordPolicy.ExpiryDays > 0 && logDate.HasValue)
                                        {
                                            dateTime1 = dateTime2;
                                            nullable = logDate;
                                            if ((nullable.HasValue ? dateTime1 >= nullable.GetValueOrDefault() ? 1 : 0 : 0) != 0)
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
                var deviceLogin2 = deviceLogin1;
                var success = deviceLogin1.Success;
                var nullable1 = new bool?(success.HasValue ? success.GetValueOrDefault() : string.IsNullOrEmpty(FormErrorText));
                deviceLogin2.Success = nullable1;
                deviceLogin1.DepositorEnabled = dbApplicationUser.DepositorEnabled;
                deviceLogin1.ChangePassword = new bool?(!dbApplicationUser.IsAdUser && dbApplicationUser.PasswordResetRequired);
            }
            _depositorDBContext.SaveChangesAsync().Wait();
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
            var callBackDelegate = LoginSuccessCallBackDelegate;
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
            var userLoginViewModel = this;
            using var depositorDBContext = new DepositorDBContext();
            var device = userLoginViewModel.ApplicationViewModel.ApplicationModel.GetDeviceAsync();
            if (user.IsAdUser || !ApplicationViewModel.DeviceConfiguration.ALLOW_OFFLINE_AUTH)
            {
                var authenticationRequest = new AuthenticationRequest
                {
                    AppID = device.AppId,
                    AppName = device.MachineName
                };
                var guid = Guid.NewGuid();
                authenticationRequest.SessionID = guid.ToString();
                authenticationRequest.Language = userLoginViewModel.ApplicationViewModel.CurrentLanguage;
                guid = Guid.NewGuid();
                authenticationRequest.MessageID = guid.ToString();
                authenticationRequest.IsADUser = user.IsAdUser;
                authenticationRequest.MessageDateTime = DateTime.Now;
                authenticationRequest.Username = user.Username;
                authenticationRequest.Password = password.Encrypt(device.AppKey);
                var request = authenticationRequest;
                var authenticationServiceClient = new AuthenticationServiceClient(ApplicationViewModel.DeviceConfiguration.API_AUTH_API_URI, device.AppId, device.AppKey, null);
                ApplicationViewModel.Log.InfoFormat(nameof(UserLoginViewModel), nameof(AuthenticationAsync), "Request", "Sending request {0}", request.ToString());
                var authenticationResponse = await authenticationServiceClient.AuthenticateAsync(request);
                ApplicationViewModel.Log.DebugFormat(nameof(UserLoginViewModel), nameof(AuthenticationAsync), "Response", "Received response {0}", authenticationResponse);
                if (authenticationResponse.IsSuccess)
                    ApplicationViewModel.Log.InfoFormat(nameof(UserLoginViewModel), nameof(AuthenticationAsync), "Login", "Login SUCCESS for request {0}, User {1}", request.MessageID, user.Username);
                else
                    ApplicationViewModel.Log.WarningFormat(nameof(UserLoginViewModel), nameof(AuthenticationAsync), "Login", "Login FAIL for request {0}, User {1}: {2}", request.MessageID, user.Username, authenticationResponse.ServerErrorMessage);
                _depositorDBContext.SaveChangesAsync().Wait();
                return authenticationResponse;
            }
            var flag = userLoginViewModel.StandardAuthentication(user);
            var authenticationResponse1 = new AuthenticationResponse
            {
                AppID = device.AppId,
                AppName = device.MachineName,
                SessionID = Guid.NewGuid().ToString()
            };
            var guid1 = Guid.NewGuid();
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

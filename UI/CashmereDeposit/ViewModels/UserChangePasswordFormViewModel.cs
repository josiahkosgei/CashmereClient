
//.UserChangePasswordFormViewModel




using Caliburn.Micro;
using Cashmere.API.Messaging.Authentication;
using Cashmere.API.Messaging.Authentication.Clients;
using Cashmere.Library.Standard.Security;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;


namespace CashmereDeposit.ViewModels
{
    internal class UserChangePasswordFormViewModel : FormViewModelBase
    {
        public bool IsAuthorise;

        private ApplicationUser User { get; set; }

        private DepositorDBContext DBContext { get; set; }

        private string OldPassword { get; set; }

        private string NewPassword { get; set; }

        private string ConfirmPassword { get; set; }

        private object NextObject { get; set; }

        private UserLoginViewModel.LoginSuccessCallBack LoginSuccessCallBackDelegate { get; set; }
        public UserChangePasswordFormViewModel(
          ApplicationViewModel applicationViewModel,
          ApplicationUser user,
          DepositorDBContext dbcontext,
          Conductor<Screen> conductor,
          Screen callingObject,
          object nextObject,
          bool isAuthorise = false,
          UserLoginViewModel.LoginSuccessCallBack loginSuccessCallBack = null)
          : base(applicationViewModel, conductor, callingObject, true)
        {
            if (dbcontext == null)
            {
                DBContext = new DepositorDBContext();
                User = DBContext.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
            }
            else
            {
                DBContext = dbcontext;
                User = user;
            }
            NextObject = nextObject;
            if (!(Application.Current.FindResource("UserChangePasswordFormScreenTitle") is string str))
                str = "Change Password";
            ScreenTitle = str;
            ScreenTitle = ScreenTitle + ": " + User.Username;
            IsAuthorise = isAuthorise;
            LoginSuccessCallBackDelegate = loginSuccessCallBack;
            var fields1 = Fields;
            var formListItem1 = new FormListItem();

            if (!(Application.Current.FindResource("User_OldPassword_Caption") is string))
                str = "Old Password";
            formListItem1.DataLabel = str;
            formListItem1.Validate = new Func<string, string>(ValidateOldPassword);
            formListItem1.ValidatedText = OldPassword;
            formListItem1.DataTextBoxLabel = OldPassword;
            formListItem1.FormListItemType = FormListItemType.ALPHAPASSWORD;
            fields1.Add(formListItem1);
            var fields2 = Fields;
            var formListItem2 = new FormListItem();
            if (!(Application.Current.FindResource("User_NewPassword_Caption") is string))
                str = "New Password";
            formListItem2.DataLabel = str;
            formListItem2.Validate = new Func<string, string>(ValidateNewPassword);
            formListItem2.ValidatedText = NewPassword;
            formListItem2.DataTextBoxLabel = NewPassword;
            formListItem2.FormListItemType = FormListItemType.ALPHAPASSWORD;
            fields2.Add(formListItem2);
            var fields3 = Fields;
            var formListItem3 = new FormListItem();
            if (!(Application.Current.FindResource("User_ConfirmPassword_Caption") is string))
                str = "Confirm Password";
            formListItem3.DataLabel = str;
            formListItem3.Validate = new Func<string, string>(ValidateConfirmPassword);
            formListItem3.ValidatedText = ConfirmPassword;
            formListItem3.DataTextBoxLabel = ConfirmPassword;
            formListItem3.FormListItemType = FormListItemType.ALPHAPASSWORD;
            fields3.Add(formListItem3);
            ActivateItemAsync(new FormListViewModel(this));
        }

        public string ValidateOldPassword(string oldPassword)
        {
            if (string.IsNullOrWhiteSpace(oldPassword))
                return "Current Password Cannot be blank";
            OldPassword = oldPassword;
            return null;
        }

        public string ValidateNewPassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                return "New Password Cannot be blank";
            NewPassword = newPassword;
            return null;
        }

        public string ValidateConfirmPassword(string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(confirmPassword))
                return "Confirm Password Cannot be blank";
            ConfirmPassword = confirmPassword;
            return null;
        }

        public override async Task<string> SaveForm()
        {
            var num = FormValidation();
            if (num > 0)
            {
                FormErrorText = string.Format("Form validation failed with {0} errors. Kindly correct them and try again", num);
                return FormErrorText;
            }
            if (ValidatePassword() == 0)
                return null;
            NewPassword = null;
            OldPassword = null;
            ConfirmPassword = null;
            foreach (var field in Fields)
            {
                field.ValidatedText = null;
                field.DataTextBoxLabel = null;
            }
            return FormErrorText;
        }

        public override int FormValidation()
        {
            var num = 0;
            foreach (var field in Fields)
            {
                field.ErrorMessageTextBlock = null;
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

        private int ValidatePassword()
        {
            try
            {
                if (!ApplicationViewModel.DeviceConfiguration.ALLOW_OFFLINE_AUTH)
                {
                    Device? device = ApplicationViewModel.GetDeviceAsync().ContinueWith(x => x.Result).Result;
                    var changePasswordRequest = new ChangePasswordRequest
                    {
                        AppID = device.AppId,
                        AppName = device.MachineName,
                        SessionID = Guid.NewGuid().ToString(),
                        DeviceID = device.Id,
                        Language = ApplicationViewModel.CurrentLanguage,
                        MessageID = Guid.NewGuid().ToString(),
                        MessageDateTime = DateTime.Now,
                        UserId = User.Id,
                        Username = User.Username,
                        OldPassword = OldPassword.Encrypt(device.AppKey),
                        NewPassword = NewPassword.Encrypt(device.AppKey),
                        ConfirmPassword = ConfirmPassword.Encrypt(device.AppKey)
                    };
                    var request = changePasswordRequest;
                    var client = new AuthenticationServiceClient(ApplicationViewModel.DeviceConfiguration.API_AUTH_API_URI, device.AppId, device.AppKey, null);
                    ApplicationViewModel.Log.InfoFormat("UserLoginViewModel", nameof(ValidatePassword), "API", "Sending request {0}", request);
                    var result = Task.Run((Func<Task<ChangePasswordResponse>>)(() => client.ChangePasswordAsync(request))).Result;
                    ApplicationViewModel.Log.DebugFormat("UserLoginViewModel", nameof(ValidatePassword), "API", "Received response {0}", result);
                    if (result.IsSuccess)
                        ApplicationViewModel.Log.InfoFormat("UserLoginViewModel", nameof(ValidatePassword), "SUCCESS", "Change password SUCCESS for request {0}, User {1}", request.MessageID, User.Username);
                    else
                        ApplicationViewModel.Log.WarningFormat("UserLoginViewModel", nameof(ValidatePassword), "FAIL", "Change password FAIL for request {0}, User {1}: {2}>{3}", request.MessageID, User.Username, result.PublicErrorMessage, result.ServerErrorMessage);
                    ApplicationViewModel.SaveToDatabaseAsync(DBContext).Wait();
                    FormErrorText = result.PublicErrorMessage;
                    return result.IsSuccess ? 0 : 1;
                }
                var passwordPolicy = DBContext.PasswordPolicies.FirstOrDefault();
                if (passwordPolicy == null)
                {
                    FormErrorText = "error, no password policy defined";
                    return 1;
                }
                var Policy = new PasswordPolicyItems()
                {
                    HistorySize = passwordPolicy.HistorySize,
                    LowerCaseLength = passwordPolicy.MinLowercase,
                    MinimumLength = passwordPolicy.MinLength,
                    NumericLength = passwordPolicy.MinDigits,
                    SpecialLength = passwordPolicy.MinSpecial,
                    UpperCaseLength = passwordPolicy.MinUppercase
                };
                IList<PasswordHistory> list = DBContext.PasswordHistories.Where(x => x.User == User.Id).OrderByDescending(x => x.LogDate).Take(passwordPolicy.HistorySize).ToList();
                if (!string.IsNullOrWhiteSpace(OldPassword) && !string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    if (PasswordStorage.VerifyPassword(OldPassword, User.Password))
                    {
                        if (NewPassword.ToUpper() == ConfirmPassword.ToUpper())
                        {
                            var passwordPolicyResultList = PasswordPolicyManager.Validate(NewPassword, Policy);
                            if (passwordPolicyResultList == null)
                            {
                                if ((bool)passwordPolicy.UseHistory && (bool)(list != null))
                                {
                                    foreach (var passwordHistory in list)
                                    {
                                        if (PasswordStorage.VerifyPassword(NewPassword, passwordHistory.Password))
                                        {
                                            FormErrorText = string.Format("Invalid: Cannot use the last {0} passwords", passwordPolicy.HistorySize);
                                            return 1;
                                        }
                                    }
                                }
                                var hash = PasswordStorage.CreateHash(NewPassword);
                                User.Password = hash;
                                User.PasswordResetRequired = false;
                                User.PasswordHistories.Add(new PasswordHistory()
                                {
                                    LogDate = new DateTime?(DateTime.Now),
                                    Id = Guid.NewGuid(),
                                    Password = hash
                                });
                                ApplicationViewModel.SaveToDatabaseAsync(DBContext).Wait();
                                return 0;
                            }
                            var stringBuilder = new StringBuilder();
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.MINIMUM_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must be at least {0:#} characters long", passwordPolicy.MinLength));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.UPPER_CASE_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must have at least {0:#} UPPERCASE characters", passwordPolicy.MinUppercase));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.LOWER_CASE_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must have at least {0:#} lowercase characters", passwordPolicy.MinLowercase));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.SPECIAL_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must have at least {0:#} special characters from {1}", passwordPolicy.MinSpecial, passwordPolicy.AllowedSpecial));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.NUMERIC_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must have at least {0:#} numeric characters 1234567890", passwordPolicy.MinDigits));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.HISTORY))
                                stringBuilder.AppendLine(string.Format("Cannot use one of the last {0:#} passwords", passwordPolicy.HistorySize));
                            FormErrorText = stringBuilder.ToString();
                        }
                        else
                        {
                            FormErrorText = "Invalid Confirm Password";
                            return 1;
                        }
                    }
                    else
                    {
                        FormErrorText = "Current Password is incorrect";
                        return 1;
                    }
                }
                else
                    FormErrorText = "Empty fields detected";
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(UserChangePasswordFormViewModel), "Error", nameof(ValidatePassword), ex.MessageString(), Array.Empty<object>());
                FormErrorText = "Error. Contact Administrator.";
                return 1;
            }
            return 1;
        }

        public override void FormClose(bool success)
        {
            if (success)
            {
                Conductor.ActivateItemAsync(NextObject);
                var callBackDelegate = LoginSuccessCallBackDelegate;
                if (callBackDelegate == null)
                    return;
                callBackDelegate(User, IsAuthorise);
            }
            else
                Conductor.ActivateItemAsync(CallingObject);
        }
    }
}

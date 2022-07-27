
// Type: CashmereDeposit.Utils.AlertClasses.AlertLoginFailed




using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.ViewModels;

namespace CashmereDeposit.Utils.AlertClasses
{
    internal class AlertLoginFailed : AlertBase
    {
        public const int ALERT_ID = 3003;
        private string _errorMessage;
        private ApplicationUser _user;
        private string _username;
        private readonly IAlertMessageTypeRepository _alertMessageTypeRepository;
        private readonly IAlertEventRepository _alertEventRepository;

        public AlertLoginFailed(
          Device device,
          DateTime dateDetected,
          string errorMessage,
          string username,
          ApplicationUser user = null)
          : base(device, dateDetected)
        {
            _user = user;
            _errorMessage = errorMessage;
            _username = username;
            _alertMessageTypeRepository = IoC.Get<IAlertMessageTypeRepository>();
            _alertEventRepository = IoC.Get<IAlertEventRepository>();
            AlertType = _alertMessageTypeRepository.GetByIdAsync(ALERT_ID);//3003);
        }

        public override bool SendAlert()
        {
            try
            {

                GenerateTokens();
                AlertEvent entity = new AlertEvent
                {
                    Id = GuidExt.UuidCreateSequential(),
                    Created = DateTime.Now,
                    AlertTypeId = AlertType.Id,
                    MachineName = Environment.MachineName.ToUpperInvariant(),
                    DateDetected = DateDetected,
                    DateResolved = DateResolved,
                    DeviceId = Device.Id,
                    IsResolved = true
                };

                AlertEmail email = GenerateEmail();
                if (email != null)
                    entity.AlertEmails.Add(email);
                AlertSMS sms = GenerateSMS();
                if (sms != null)
                    entity.AlertSMS.Add(sms);
                _alertEventRepository.AddAsync(entity);
                return true;
            }
            catch (ValidationException ex)
            {
                string ErrorDetail = "Error Saving to Database: " + string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message);
                foreach (var entityValidationError in ex.ValidationResult.MemberNames)
                {
                    ErrorDetail += ">validation error>";
                    ErrorDetail = ErrorDetail + "ErrorMessage=>" + entityValidationError;
                }
                Console.WriteLine(ErrorDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Saving to Database: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
            }
            return false;
        }

        private AlertEmail GenerateEmail()
        {
            AlertEmail alertEmail = new AlertEmail
            {
                Id = GuidExt.UuidCreateSequential(),
                Created = DateTime.Now,
                HtmlMessage = GetHTMLBody(),
                RawTextMessage = GetRawTextBody(),
                Subject = AlertType.Name,
                Sent = false
            };
            return alertEmail.RawTextMessage == null || alertEmail.HtmlMessage == null ? null : alertEmail;
        }

        private AlertSMS GenerateSMS()
        {
            AlertSMS alertSm = new AlertSMS
            {
                Id = GuidExt.UuidCreateSequential(),
                Created = DateTime.Now,
                Message = GetSMSBody(),
                Sent = false
            };
            return alertSm.Message == null ? null : alertSm;
        }

        private new void GenerateTokens()
        {
            Tokens = new Dictionary<string, string>();
            Tokens.Add("[date]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            Tokens.Add("[device_id]", Device.DeviceNumber);
            Tokens.Add("[device_name]", Device.Name);
            Tokens.Add("[device_location]", Device.DeviceLocation);
            Tokens.Add("[branch_name]", Device.Branch.Name);
            Tokens.Add("[event_title]", AlertType.Title);
            Tokens.Add("[event_id]", AlertType.Id.ToString() ?? "");
            Tokens.Add("[event_name]", AlertType.Name);
            Tokens.Add("[event_description]", AlertType.Description);
            Tokens.Add("[date_detected]", DateDetected.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            Tokens.Add("[date_resolved]", "");
            Tokens.Add("[resolution_duration]", "");
            Tokens.Add("[login_username]", _username);
            Tokens.Add("[login_error]", _errorMessage);
            Tokens.Add("[login_user_fname]", _user?.Fname);
            Tokens.Add("[login_user_lname]", _user?.Lname);
            Tokens.Add("[login_user_username]", _user?.Username);
            Tokens.Add("[login_user_email]", _user?.Email);
            Tokens.Add("[login_user_phone]", _user?.Phone);
            IDictionary<string, string> tokens = Tokens;
            ApplicationUser user = _user;
            bool? nullable1;
            bool? nullable2;
            if (user == null)
            {
                nullable2 = new bool?();
            }
            else
            {
                nullable1 = user.DepositorEnabled;
                nullable2 = nullable1.HasValue ? !nullable1.GetValueOrDefault() : new bool?();
            }
            nullable1 = nullable2;
            string str = nullable1.ToString();
            tokens.Add("[login_user_islocked]", str);
            Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
            Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
            Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
        }

        protected new string GenerateHTMLMessageToken() => "";

        protected new string GenerateRawTextMessageToken() => "";

        protected new string GenerateSMSMessageToken() => "";

        private new string GetHTMLBody()
        {
            string str = AlertType.EmailContentTemplate;
            if (str != null)
            {
                foreach (KeyValuePair<string, string> token in Tokens)
                    str = str.Replace(token.Key, token.Value);
            }
            return str;
        }

        private new string GetRawTextBody()
        {
            string str = AlertType.RawEmailContentTemplate;
            if (str != null)
            {
                foreach (KeyValuePair<string, string> token in Tokens)
                    str = str.Replace(token.Key, token.Value);
            }
            return str;
        }

        private new string GetSMSBody()
        {
            string str = AlertType?.PhoneContentTemplate;
            if (str != null)
            {
                foreach (KeyValuePair<string, string> token in Tokens)
                    str = str.Replace(token.Key, token.Value);
            }
            return str;
        }

        private new AlertEvent GetCorrespondingAlertEvent() => throw new NotImplementedException();
    }
}

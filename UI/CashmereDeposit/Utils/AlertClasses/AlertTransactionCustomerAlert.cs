
// Type: CashmereDeposit.Utils.AlertClasses.AlertTransactionCustomerAlert




using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.Models;
using CashmereDeposit.ViewModels;

namespace CashmereDeposit.Utils.AlertClasses
{
    public class AlertTransactionCustomerAlert : AlertBase
    {
        public const int ALERT_ID = 4009;
        private AppTransaction _transaction;

        public AlertTransactionCustomerAlert(
          AppTransaction transaction,
          Device device,
          DateTime dateDetected)
          : base(device, dateDetected)
        {
            _transaction = transaction != null ? transaction : throw new NullReferenceException("Variable transaction cannot be null in " + GetType().Name);
            using DepositorDBContext depositorDbContext = new DepositorDBContext();
            AlertType = depositorDbContext.AlertMessageTypes.FirstOrDefault(x => x.Id == 4009);
        }

        public override bool SendAlert()
        {
            try
            {
                using DepositorDBContext DBContext = new DepositorDBContext();
                GenerateTokens();
                AlertEvent entity = new AlertEvent()
                {
                    Id = GuidExt.UuidCreateSequential(),
                    Created = DateTime.Now,
                    AlertTypeId = AlertType.Id,
                    MachineName = Environment.MachineName.ToUpperInvariant(),
                    DateDetected = DateDetected,
                    DateResolved = new DateTime?(DateResolved),
                    DeviceId = Device.Id,
                    IsResolved = true
                };
                DBContext.AlertEvents.Add(entity);
                AlertEmail email = GenerateEmail();
                if (email != null)
                    entity.AlertEmails.Add(email);
                AlertSMS sms = GenerateSMS();
                if (sms != null)
                    entity.AlertSMS.Add(sms);
                ApplicationViewModel.SaveToDatabaseAsync(DBContext).Wait();
                return true;
            }
            catch (ValidationException ex)
            {
                string ErrorDetail = "Error Saving to Database: " + string.Format("{0}\n{1}", (object)ex.Message, (object)ex?.InnerException?.Message);
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
            AlertEmail alertEmail = new AlertEmail()
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
            AlertSMS alertSm = new AlertSMS()
            {
                Id = GuidExt.UuidCreateSequential(),
                Created = DateTime.Now,
                Message = GetSMSBody(),
                Sent = false,
                To = _transaction.Phone
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
            Tokens.Add("[date_detected]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            Tokens.Add("[device.name]", Device.Name);
            Tokens.Add("[transaction.end_date]", _transaction.EndDate.ToString(ApplicationViewModel.DeviceConfiguration.SMS_DATE_FORMAT ?? "d/M/yy 'at' h:mm tt", CultureInfo.InvariantCulture));
            Tokens.Add("[transaction.currency]", _transaction.CurrencyCode);
            Tokens.Add("[transaction.total_amount]", _transaction.TotalDisplayAmount.ToString(ApplicationViewModel.DeviceConfiguration.SMS_AMOUNT_FORMAT ?? "#,#0.00", CultureInfo.InvariantCulture));
            Tokens.Add("[transaction.cr_account_number]", _transaction.AccountNumber);
            Tokens.Add("[transaction.depositor_name]", _transaction.DepositorName);
            Tokens.Add("[transaction.narration]", _transaction.Narration);
            Tokens.Add("[transaction.cb_tx_number]", _transaction.Transaction.CbTxNumber);
            Tokens.Add("[transaction.cr_account_name]", _transaction.AccountName);
            Tokens.Add("[transaction.branch_name]", _transaction.BranchName);
            Tokens.Add("[transaction.device_number]", _transaction.DeviceNumber);
            Tokens.Add("[transaction.funds_source]", _transaction.FundsSource);
            Tokens.Add("[transaction.id_number]", _transaction.IDNumber);
            Tokens.Add("[transaction.phone]", _transaction.Phone);
            Tokens.Add("[transaction.ref_account_number]", _transaction.ReferenceAccount);
            Tokens.Add("[transaction.ref_account_name]", _transaction.ReferenceAccountName);
            Tokens.Add("[transaction.start_date]", _transaction.StartDate.ToString(ApplicationViewModel.DeviceConfiguration.SMS_DATE_FORMAT ?? "d/M/yy 'at' h:mm tt", CultureInfo.InvariantCulture));
            Tokens.Add("[transaction.dr_account_number]", _transaction.SuspenseAccount);
            Tokens.Add("[transaction.transaction_type]", _transaction.TransactionType?.Name);
            Tokens.Add("[bank.name]", Device.Branch.Bank.Name);
            Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
            Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
            Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
        }

        protected new string GenerateHTMLMessageToken() => _transaction.ToEmailString();

        protected new string GenerateRawTextMessageToken() => _transaction.ToRawTextString();

        protected new string GenerateSMSMessageToken() => null;

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
            string EventDetail = AlertType?.PhoneContentTemplate;
            if (EventDetail != null)
            {
                foreach (KeyValuePair<string, string> token in Tokens)
                    EventDetail = EventDetail.Replace(token.Key, token.Value);
            }
            ApplicationViewModel.Log.Info(nameof(AlertTransactionCustomerAlert), "Generated SMS Body", nameof(GetSMSBody), EventDetail);
            return EventDetail;
        }

        private new AlertEvent GetCorrespondingAlertEvent(DepositorDBContext DBContext) => throw new NotImplementedException();
    }
}

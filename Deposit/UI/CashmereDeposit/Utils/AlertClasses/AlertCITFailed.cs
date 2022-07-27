
// Type: CashmereDeposit.Utils.AlertClasses.AlertCITFailed




using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.ViewModels;

namespace CashmereDeposit.Utils.AlertClasses
{
    public class AlertCITFailed : AlertBase
    {
        public const int ALERT_ID = 1300;
        private CIT _cit;
        private readonly IAlertMessageTypeRepository _alertMessageTypeRepository;
        private readonly IAlertEventRepository _alertEventRepository;

        public AlertCITFailed(CIT CIT, Device device, DateTime dateDetected)
          : base(device, dateDetected)
        {
            _cit = CIT != null ? CIT : throw new NullReferenceException("Variable CIT cannot be null");
            _alertMessageTypeRepository = IoC.Get<IAlertMessageTypeRepository>();
            _alertEventRepository = IoC.Get<IAlertEventRepository>();
            AlertType = _alertMessageTypeRepository.GetByIdAsync(ALERT_ID);//== 1300);
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
            IDictionary<string, string> tokens1 = Tokens;
            int num = AlertType.Id;
            string str1 = num.ToString() ?? "";
            tokens1.Add("[event_id]", str1);
            Tokens.Add("[event_name]", AlertType.Name);
            Tokens.Add("[event_description]", AlertType.Description);
            Tokens.Add("[CIT_date]", _cit.CITDate.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
            IDictionary<string, string> tokens2 = Tokens;
            DateTime? nullable = _cit.FromDate;
            DateTime dateTime;
            string str2;
            if (!nullable.HasValue)
            {
                dateTime = DateTime.MinValue;
                str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            }
            else
            {
                nullable = _cit.FromDate;
                dateTime = nullable.Value;
                str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            }
            tokens2.Add("[CIT_start]", str2);
            IDictionary<string, string> tokens3 = Tokens;
            dateTime = _cit.ToDate;
            string str3 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            tokens3.Add("[CIT_end]", str3);
            IDictionary<string, string> tokens4 = Tokens;
            nullable = _cit.CITCompleteDate;
            string str4;
            if (!nullable.HasValue)
            {
                str4 = null;
            }
            else
            {
                nullable = _cit.CITCompleteDate;
                dateTime = nullable.Value;
                str4 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            }
            tokens4.Add("[CIT_completed_date]", str4);
            Tokens.Add("[user_init]", _cit.StartUserNavigation.Username);
            Tokens.Add("[user_auth]", _cit.AuthUserNavigation.Username);
            Tokens.Add("[old_bag_number]", _cit.OldBagNumber);
            Tokens.Add("[new_bag_number]", _cit.NewBagNumber);
            Tokens.Add("[seal_number]", _cit.SealNumber);
            IDictionary<string, string> tokens5 = Tokens;
            num = _cit.CITError;
            string str5 = num.ToString();
            tokens5.Add("[error_code]", str5);
            Tokens.Add("[error_message]", _cit.CITErrorMessage);
            Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
            Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
            Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
        }

        protected new string GenerateHTMLMessageToken()
        {
            StringBuilder stringBuilder = new StringBuilder(byte.MaxValue);
            foreach (IGrouping<Currency, CITDenomination> source in _cit.CITDenominations.GroupBy(x => x.Currency))
            {
                Currency groupKey = source.Key;
                stringBuilder.AppendLine(string.Format("<hr /><h3>Currency: {0}</h3><hr />", groupKey.Code.ToUpper()));
                stringBuilder.AppendLine(string.Format("Transaction Count:{0}<br />", _cit.Transactions.Where(x => x.TxCurrencyNavigation.Code == groupKey.Code).Count()));
                stringBuilder.AppendLine("<table style=\"text - align: left\">");
                stringBuilder.AppendLine("<tr><th>Denomination</th><th>Count</th><th>Sub Total</th></tr>");
                foreach (CITDenomination citDenomination in source)
                {
                    double num1 = citDenomination.Denom / 100.0;
                    long count = citDenomination.Count;
                    double num2 = citDenomination.Denom * citDenomination.Count / 100.0;
                    int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
                    stringBuilder.AppendLine(string.Format("<tr><td>{0:##,0.##}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", num1, count, num2));
                }
                stringBuilder.AppendLine(string.Format("<tr><td>{0}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", "TOTAL:", source.Sum(x => x.Count), source.Sum(x => x.Subtotal) / 100.0));
                stringBuilder.AppendLine("</table>");
            }
            return stringBuilder.ToString();
        }

        protected new string GenerateRawTextMessageToken()
        {
            StringBuilder stringBuilder = new StringBuilder(byte.MaxValue);
            foreach (IGrouping<Currency, CITDenomination> source in _cit.CITDenominations.GroupBy(x => x.Currency))
            {
                Currency groupKey = source.Key;
                stringBuilder.AppendLine("----------------------------------------");
                stringBuilder.AppendLine(string.Format("Currency: {0}", groupKey.Code.ToUpper()));
                stringBuilder.AppendLine(string.Format("Transaction Count:{0}", _cit.Transactions.Where(x => x.TxCurrencyNavigation.Code == groupKey.Code).Count()));
                stringBuilder.AppendLine("----------------------------------------");
                stringBuilder.AppendLine(string.Format("{0,-10}{1,7}{2,21}", "Denomination", "Count", "Sub Total"));
                stringBuilder.AppendLine("________________________________________");
                foreach (CITDenomination citDenomination in source)
                {
                    double num1 = citDenomination.Denom / 100.0;
                    long count = citDenomination.Count;
                    double num2 = citDenomination.Denom * citDenomination.Count / 100.0;
                    int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
                    stringBuilder.AppendLine(string.Format("{0,-10:0.##}{1,7:##,0}{2,23:##,0.##}", num1, count, num2));
                }
                stringBuilder.AppendLine("========================================");
                stringBuilder.AppendLine(string.Format("{0,-10}{1,7:##,0}{2,23:##,0.##}", "TOTAL:", source.Sum(x => x.Count), source.Sum(x => x.Subtotal) / 100.0));
                stringBuilder.AppendLine("========================================");
            }
            return stringBuilder.ToString();
        }

        protected new string GenerateSMSMessageToken()
        {
            StringBuilder stringBuilder = new StringBuilder(byte.MaxValue);
            foreach (IGrouping<Currency, CITDenomination> source in _cit.CITDenominations.GroupBy(x => x.Currency))
            {
                Currency groupKey = source.Key;
                stringBuilder.AppendLine("CCY: " + groupKey.Code.ToUpper());
                stringBuilder.AppendLine(string.Format("Tx: {0}", _cit.Transactions.Where(x => x.TxCurrencyNavigation == groupKey).Count()));
                stringBuilder.AppendLine("Notes: " + source.Sum(x => x.Count));
                stringBuilder.AppendLine(string.Format("{0} Total: {1:0.##}", groupKey.Code.ToUpper(), source.Sum(x => x.Subtotal) / 100.0));
            }
            return stringBuilder.ToString();
        }

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

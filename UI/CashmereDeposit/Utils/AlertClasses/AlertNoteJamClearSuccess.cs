
// Type: CashmereDeposit.Utils.AlertClasses.AlertNoteJamClearSuccess




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
    public class AlertNoteJamClearSuccess : AlertBase
    {
        public const int ALERT_ID = 4002;
        private readonly IAlertMessageTypeRepository _alertMessageTypeRepository;
        private readonly IAlertEventRepository _alertEventRepository;
        private Transaction _transaction;
        private AlertEvent associatedAlertEvent;

        public AlertNoteJamClearSuccess(Transaction transaction, Device device, DateTime dateDetected)
          : base(device, dateDetected)
        {

            _alertMessageTypeRepository = IoC.Get<IAlertMessageTypeRepository>();
            _alertEventRepository = IoC.Get<IAlertEventRepository>();
            _transaction = transaction ?? throw new NullReferenceException("Variable transaction cannot be null in " + GetType().Name);

            associatedAlertEvent = _alertEventRepository.GetAlertEventAsync(4002).ContinueWith(x => x.Result).Result; ;
            AlertType = _alertMessageTypeRepository.GetByIdAsync(4002).ContinueWith(x => x.Result).Result;
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
                //_depositorDBContext.AlertEvents.Add(entity);
                AlertEmail email = GenerateEmail();
                if (email != null)
                    entity.AlertEmails.Add(email);
                AlertSMS sms = GenerateSMS();
                if (sms != null)
                    entity.AlertSMS.Add(sms);
                _alertEventRepository.AddAsync(entity).Wait();
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
            IDictionary<string, string> tokens1 = Tokens;
            AlertEvent associatedAlertEvent = this.associatedAlertEvent;
            DateTime dateTime;
            string str1;
            if (associatedAlertEvent == null)
            {
                str1 = null;
            }
            else
            {
                dateTime = associatedAlertEvent.Created;
                str1 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            }
            tokens1.Add("[date_detected]", str1);
            IDictionary<string, string> tokens2 = Tokens;
            dateTime = DateTime.Now;
            string str2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff");
            tokens2.Add("[date_resolved]", str2);
            Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
            Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
            Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
        }

        protected new string GenerateHTMLMessageToken()
        {
            StringBuilder stringBuilder1 = new StringBuilder(byte.MaxValue);
            stringBuilder1.AppendLine("<hr /><h3>Transaction</h3><hr />");
            StringBuilder stringBuilder2 = stringBuilder1;
            object[] objArray = new object[16]
            {
        _transaction.TxStartDate,
        _transaction.TxEndDate,
        _transaction.TxTypeNavigation.Name,
        _transaction.TxAccountNumber,
        _transaction.CbAccountName,
        _transaction.TxRefAccount,
        _transaction.CbRefAccountName,
        _transaction.TxNarration,
        _transaction.TxDepositorName,
        _transaction.TxIdNumber,
        _transaction.TxPhone,
        _transaction.TxCurrency,
        null,
        null,
        null,
        null
            };
            long? txAmount = _transaction.TxAmount;
            long num1 = 100;
            objArray[12] = txAmount.HasValue ? txAmount.GetValueOrDefault() / num1 : new long?();
            objArray[13] = _transaction.FundsSource;
            objArray[14] = _transaction.TxErrorCode;
            objArray[15] = _transaction.TxErrorMessage;
            string str = string.Format("<p><table>\r\n                <tr><th>Start Date</th><th>End Date</th><th>Transaction Type</th><th>Account Number</th><th>Account Name</th><th>Reference Account Number</th><th>Reference Account Name</th>\r\n                <th>Narration</th><th>Depositor Name</th><th>Depositor ID</th><th>Depositor Phone</th><th>Currency</th><th>Amount</th><th>Source of Funds</th>\r\n                <th>Error Code</th><th>Error Message</th></tr>\r\n                <tr><td>{0}</td>  <td>{1}</td>  <td>{2}</td>  <td>{3}</td>  <td>{4}</td>  <td>{5}</td>  <td>{6}</td>  <td>{7}</td>  <td>{8}</td>  <td>{9}</td>  <td>{10}</td><td>{11}</td><td>{12}</td><td>{13}</td><td>{14}</td><td>{15}</td></tr>\r\n</table></p>", objArray);
            stringBuilder2.AppendLine(str);
            stringBuilder1.AppendLine("<p><table style=\"text - align: left\">");
            stringBuilder1.AppendLine("<tr><th>Denomination</th><th>Count</th><th>Sub Total</th></tr>");
            List<DenominationDetail> list = _transaction.DenominationDetails.ToList();
            foreach (DenominationDetail denominationDetail in list)
            {
                double num2 = denominationDetail.Denom / 100.0;
                long count = denominationDetail.Count;
                double num3 = denominationDetail.Denom * denominationDetail.Count / 100.0;
                int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
                stringBuilder1.AppendLine(string.Format("<tr><td>{0:##,0.##}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", num2, count, num3));
            }
            stringBuilder1.AppendLine(string.Format("<tr><td>{0}</td><td>{1:##,0}</td><td>{2:##,0.##}</td></tr>", "TOTAL:", list.Sum(x => x.Count), list.Sum(x => x.Subtotal) / 100.0));
            stringBuilder1.AppendLine("</table></p>");
            return stringBuilder1.ToString();
        }

        protected new string GenerateRawTextMessageToken()
        {
            StringBuilder stringBuilder1 = new StringBuilder(byte.MaxValue);
            stringBuilder1.AppendLine("----------------------------------------");
            stringBuilder1.AppendLine("             Transaction");
            stringBuilder1.AppendLine("----------------------------------------");
            StringBuilder stringBuilder2 = stringBuilder1;
            object[] objArray = new object[16]
            {
        _transaction.TxStartDate,
        _transaction.TxEndDate,
        _transaction.TxTypeNavigation.Name,
        _transaction.TxAccountNumber,
        _transaction.CbAccountName,
        _transaction.TxRefAccount,
        _transaction.CbRefAccountName,
        _transaction.TxNarration,
        _transaction.TxDepositorName,
        _transaction.TxIdNumber,
        _transaction.TxPhone,
        _transaction.TxCurrency,
        null,
        null,
        null,
        null
            };
            long? txAmount = _transaction.TxAmount;
            long num1 = 100;
            objArray[12] = txAmount.HasValue ? txAmount.GetValueOrDefault() / num1 : new long?();
            objArray[13] = _transaction.FundsSource;
            objArray[14] = _transaction.TxErrorCode;
            objArray[15] = _transaction.TxErrorMessage;
            string str = string.Format("\r\nStart Date:                 {0}\r\nEnd Date:                   {1}\r\nTransaction Type:           {2}\r\nAccount Number:             {3}\r\nAccount Name:               {4}\r\nReference Account Number:   {5}\r\nReference Account Name:     {6}            \r\nNarration:                  {7}\r\nDepositor Name:             {8}\r\nDepositor ID:               {9}\r\nDepositor Phone:            {10}\r\nCurrency:                   {11}\r\nAmount:                     {12}\r\nSource of Funds:            {13}\r\nError Code:                 {14}\r\nError Message:              {15}", objArray);
            stringBuilder2.AppendLine(str);
            stringBuilder1.AppendLine("________________________________________");
            stringBuilder1.AppendLine(string.Format("{0,-10}{1,7}{2,21}", "Denomination", "Count", "Sub Total"));
            stringBuilder1.AppendLine("________________________________________");
            List<DenominationDetail> list = _transaction.DenominationDetails.ToList();
            foreach (DenominationDetail denominationDetail in list)
            {
                double num2 = denominationDetail.Denom / 100.0;
                long count = denominationDetail.Count;
                double num3 = denominationDetail.Denom * denominationDetail.Count / 100.0;
                int receiptWidth = ApplicationViewModel.DeviceConfiguration.RECEIPT_WIDTH;
                stringBuilder1.AppendLine(string.Format("{0,-10:0.##}{1,7:##,0}{2,23:##,0.##}", num2, count, num3));
            }
            stringBuilder1.AppendLine("========================================");
            stringBuilder1.AppendLine(string.Format("{0,-10}{1,7:##,0}{2,23:##,0.##}", "TOTAL:", list.Sum(x => x.Count), list.Sum(x => x.Subtotal) / 100.0));
            stringBuilder1.AppendLine("========================================");
            return stringBuilder1.ToString();
        }

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

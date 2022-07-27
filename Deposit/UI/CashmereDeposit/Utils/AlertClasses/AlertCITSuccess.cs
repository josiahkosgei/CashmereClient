
// Type: CashmereDeposit.Utils.AlertClasses.AlertCITSuccess




using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.Standard.Security;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.ViewModels;
using CashmereUtil.Reporting.CITReporting;
using CashmereUtil.Reporting.MSExcel;
using CIT = Cashmere.Library.CashmereDataAccess.Entities.CIT;
using CITDenomination = Cashmere.Library.CashmereDataAccess.Entities.CITDenomination;
using Transaction = Cashmere.Library.CashmereDataAccess.Entities.Transaction;

namespace CashmereDeposit.Utils.AlertClasses
{
    public class AlertCITSuccess : AlertBase
    {
        public const int ALERT_ID = 1300;
        private readonly IAlertAttachmentTypeRepository _alertAttachmentTypeRepository;
        private readonly IAlertEmailAttachmentRepository _alertEmailAttachmentRepository;
        private readonly IAlertMessageTypeRepository _alertMessageTypeRepository;
        private readonly IAlertEventRepository _alertEventRepository;
        private readonly ITransactionRepository _transactionRepository;
        private CIT _cit;

        public AlertCITSuccess(CIT CIT, Device device, DateTime dateDetected)
          : base(device, dateDetected)
        {
            _alertAttachmentTypeRepository = IoC.Get<IAlertAttachmentTypeRepository>();
            _alertMessageTypeRepository = IoC.Get<IAlertMessageTypeRepository>();
            _alertEmailAttachmentRepository = IoC.Get<IAlertEmailAttachmentRepository>();
            _alertEventRepository = IoC.Get<IAlertEventRepository>();
            _transactionRepository = IoC.Get<ITransactionRepository>();
            _cit = CIT != null ? CIT : throw new NullReferenceException("Variable CIT cannot be null");

            AlertType = _alertMessageTypeRepository.GetByIdAsync(ALERT_ID);
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
                Console.WriteLine("Error: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
            }
            return false;
        }

        private new AlertEmail GenerateEmail()
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
            try
            {
                CreateAlertEmailattachments(alertEmail);
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(AlertCITSuccess), 1, "CreateAlertEmailattachments Failed", ex.MessageString());
            }
            return alertEmail.RawTextMessage == null || alertEmail.HtmlMessage == null ? null : alertEmail;
        }

        private void CreateAlertEmailattachments(AlertEmail alertEmail)
        {
            DirectoryInfo directory = Directory.CreateDirectory(ApplicationViewModel.DeviceConfiguration.EMAIL_LOCAL_FOLDER + "\\Attachments\\CITReport\\");
            GenerateCITReportAttachment(alertEmail, directory);
        }

        private async void GenerateCITReportAttachment(

          AlertEmail alertEmail,
          DirectoryInfo directory)
        {
            AlertAttachmentType alertAttachmentType =  _alertAttachmentTypeRepository.GetByCode("130001");
            if (alertAttachmentType == null)
                return;
            if (!alertAttachmentType.Enabled)
            {
                ApplicationViewModel.AlertLog.WarningFormat(nameof(AlertCITSuccess), "AttachmentType Disabled", "CreateAlertEmailattachments", "Attachment type {0} is disabled", alertAttachmentType.Name);
            }
            else
            {
                ApplicationViewModel.AlertLog.TraceFormat(nameof(AlertCITSuccess), "AttachmentType Enabled", "CreateAlertEmailattachments", "Attachment type {0} is enabled", alertAttachmentType.Name);
                ApplicationViewModel.AlertLog.Trace(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "create the CIT Report");
                CITReport CITReport = new CITReport
                {
                    CIT = new CashmereUtil.Reporting.CITReporting.CIT
                    {
                        id = _cit.Id,
                        CIT_complete_date = _cit.CITCompleteDate,
                        CIT_date = _cit.CITDate,
                        Error = _cit.CITError.ToString("0"),
                        ErrorMessage = _cit.CITErrorMessage,
                        Complete = _cit.Complete,
                        device_id = _cit.DeviceId,
                        device = _cit.Device.Name,
                        FromDate = _cit.FromDate,
                        ToDate = _cit.ToDate,
                        NewBagNumber = _cit.NewBagNumber,
                        OldBagNumber = _cit.OldBagNumber,
                        SealNumber = _cit.SealNumber,
                        InitiatingUser = _cit?.StartUserNavigation?.Username,
                        AuthorisingUser = _cit?.AuthUserNavigation?.Username
                    }
                };
                ApplicationViewModel.AlertLog.TraceFormat(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "Add Txns to CITReport");
                CITReport.Transactions = new List<CashmereUtil.Reporting.CITReporting.Transaction>(_cit.Transactions.Count());
                foreach (Transaction transaction in _cit.Transactions.Where(x =>
                         {
                             if (x.TxErrorCode != 1010)
                                 return true;
                             if (x.TxErrorCode != 1011)
                                 return false;
                             long? txAmount = x.TxAmount;
                             long num = 0;
                             return txAmount.GetValueOrDefault() > num & txAmount.HasValue;
                         }).OrderBy(y => y.TxStartDate))
                {
                    Transaction x = transaction;
                    CITReport.Transactions.Add(new CashmereUtil.Reporting.CITReporting.Transaction
                    {
                        id = x.Id,
                        AccountName = x.CbAccountName,
                        AccountNumber = x.TxAccountNumber,
                        Amount = (x.TxAmount.HasValue ? x.TxAmount.Value : 0.0M) / 100.0M,
                        Currency = x.TxCurrency,
                        StartTime = x.TxStartDate,
                        EndTime = x.TxEndDate.Value,
                        DepositorIDNumber = x.TxIdNumber,
                        DepositorName = x.TxDepositorName,
                        DepositorPhone = x.TxPhone,
                        DeviceID = x.DeviceId,
                        DeviceName = x.Device.Name,
                        DeviceReferenceNumber = x.TxRandomNumber.Value.ToString("0"),
                        CB_Reference = x.CbTxNumber,
                        FundsSource = x.FundsSource,
                        Narration = x.TxNarration,
                        RefAccountName = x.CbRefAccountName,
                        RefAccountNumber = x.TxRefAccount,
                        SuspenseAccount = x.Device.DeviceCITSuspenseAccounts.FirstOrDefault(t => t.DeviceId == x.DeviceId && t.CurrencyCode == x.TxCurrency)?.AccountNumber,
                        TransactionType = x.TxTypeNavigation?.Name,
                        ErrorCode = x.TxErrorCode,
                        ErrorMessage = x.TxErrorMessage
                    });
                }
                CITReport.EscrowJams = _cit.Transactions.SelectMany(tx => tx.EscrowJams).Select(jam => new CashmereUtil.Reporting.CITReporting.EscrowJam
                {
                    id = jam.Id,
                    AdditionalInfo = jam.AdditionalInfo,
                    AuthorisingUser = jam.AuthorisingUserNavigation.Username,
                    InitialisingUser = jam.InitialisingUserNavigation.Username,
                    DateDetected = jam.DateDetected,
                    DroppedAmount = jam.DroppedAmount / 100M,
                    EscrowAmount = jam.EscrowAmount / 100M,
                    PostedAmount = jam.PostedAmount / 100M,
                    RetreivedAmount = jam.RetreivedAmount / 100M,
                    RecoveryDate = jam.RecoveryDate,
                    AccountNumber = jam.Transaction.TxAccountNumber,
                    DeviceReferenceNumber = string.Format("{0:0}", jam.Transaction.TxRandomNumber),
                    transaction_id = jam.TransactionId,
                    CB_Reference = jam.Transaction.CbTxNumber,
                    StartTime = jam.Transaction.TxStartDate,
                    EndTime = jam.Transaction.TxEndDate
                }).ToList();
                ApplicationViewModel.AlertLog.TraceFormat(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "Add CITDenoms to CITReport");
                CITReport.CITDenominations = new List<CashmereUtil.Reporting.CITReporting.CITDenomination>(_cit.CITDenominations.Count());
                foreach (CITDenomination citDenomination in _cit.CITDenominations)
                    CITReport.CITDenominations.Add(new CashmereUtil.Reporting.CITReporting.CITDenomination
                    {
                        Currency = citDenomination.Currency.Code,
                        Denom = citDenomination.Denom / 100.0M,
                        Count = citDenomination.Count,
                        SubTotal = citDenomination.Subtotal / 100.0M
                    });
                ApplicationViewModel.AlertLog.DebugFormat(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "CITReport created SUCCESS");
                string path2 = string.Format("CITReport_{0:yyyy-MM-dd HH.mm.ss.fff}.xlsx", CITReport.CIT.CIT_date);
                string str = Path.Combine(directory.FullName, path2);
                byte[] citExcelAttachment = ExcelManager.GenerateCITExcelAttachment(CITReport, str);
                FileInfo fileInfo = new FileInfo(str);
                ApplicationViewModel.AlertLog.InfoFormat(nameof(AlertCITSuccess), "CITReport", "CreateAlertEmailattachments", "CITReport attachment saved at {0} with {1} bytes", str, fileInfo.Length.ToString("#,##0"));
                 _alertEmailAttachmentRepository.AddAsync(new AlertEmailAttachment
                {
                    Id = Guid.NewGuid(),
                    AlertEmailId = alertEmail.Id,
                    Name = path2,
                    Path = str,
                    Type = alertAttachmentType.Code,
                    Data = citExcelAttachment,
                    Hash = CashmereHashing.HMACSHA512(Device.AppKey, citExcelAttachment)
                });
                // _alertEventRepository.AddAsync(entity);
            }
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

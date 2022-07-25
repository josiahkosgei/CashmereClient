
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.API.Messaging.Communication.Clients;
using Cashmere.API.Messaging.Communication.Emails;
using Cashmere.API.Messaging.Communication.SMSes;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.ViewModels;
using Microsoft.EntityFrameworkCore;
using Caliburn.Micro;

namespace CashmereDeposit.Models
{
    internal class DepositorCommunicationService : IDisposable
    {
        private static DepositorCommunicationService _instance;
        private static ICashmereLogger _log;
        private static string _commservUri;
        private static Guid _appId;
        private static byte[] _appKey;
        private static string _appName;
        private BackgroundWorker _emailSendingWorker = new BackgroundWorker();
        private bool _quitSendingEmail = true;
        private int _alertBatchSize = Math.Max(ApplicationViewModel.DeviceConfiguration.ALERT_BATCH_SIZE, 5);

        private static IHttpClientFactory _httpClientFactory;
        private readonly IAlertMessageTypeRepository _alertMessageTypeRepository;
        private readonly IAlertEmailAttachmentRepository _alertEmailAttachmentRepository;
        private readonly IAlertEventRepository _alertEventRepository;
        private static IDeviceRepository _deviceRepository;
        private static IAlertSMSRepository _alertSMSRepository;
        private static IAlertEmailRepository _alertEmailRepository;

        private DepositorCommunicationService()
        {
            _alertMessageTypeRepository = IoC.Get<IAlertMessageTypeRepository>();
            _alertEventRepository = IoC.Get<IAlertEventRepository>();
            _deviceRepository = IoC.Get<IDeviceRepository>();
            _alertSMSRepository = IoC.Get<IAlertSMSRepository>();
            _alertEmailAttachmentRepository = IoC.Get<IAlertEmailAttachmentRepository>();
            _alertEmailRepository = IoC.Get<IAlertEmailRepository>();
            _emailSendingWorker.DoWork += new DoWorkEventHandler(EmailSendingWorker_DoWork);
            _emailSendingWorker.RunWorkerAsync();
        }


        public static DepositorCommunicationService GetDepositorCommunicationService() => _instance;

        public static DepositorCommunicationService NewDepositorCommunicationService(
          string commServUri,
          Guid appId,
          byte[] appKey,
          string appName)
        {
            if (!ApplicationViewModel.DeviceConfiguration.EMAIL_CAN_SEND && !ApplicationViewModel.DeviceConfiguration.SMS_CAN_SEND)
                return null;
            if (_instance != null)
                _instance.Dispose();
            if (_instance == null)
            {
                _log = new CashmereLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), nameof(DepositorCommunicationService), null);
                _appId = appId;
                _appKey = appKey;
                _appName = appName;
                _commservUri = commServUri;
                _instance = new DepositorCommunicationService();
            }
            return GetDepositorCommunicationService();
        }

        private async void EmailSendingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _log.Debug(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "EmailSendingWorker_DoWork started");
            while (!_quitSendingEmail)
            {
                try
                {

                    if (ApplicationViewModel.DeviceConfiguration.EMAIL_CAN_SEND || ApplicationViewModel.DeviceConfiguration.SMS_CAN_SEND)
                    {
                        List<AlertEvent> list = await _alertEventRepository.GetUnProcessedAsync(_alertBatchSize);
                        _log.Trace(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "Found {0} AlertEvents to process", list.Count());
                        foreach (AlertEvent alertEvent in list)
                        {
                            try
                            {
                                _log.Info(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), string.Format("processing AlertEvent {0}", alertEvent));
                                ProcessEmail(alertEvent);
                                ProcessSms(alertEvent);
                                _log.Info(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "alertEvent [{0}] processed SUCCESS", alertEvent.ToString());
                                alertEvent.IsProcessed = true;
                            }
                            catch (NullReferenceException ex)
                            {
                                _log.Info(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "alertEvent [{0}] processed ERROR", alertEvent.ToString());
                                _log.Error(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), "NullReferenceException", "Error processing alertEvent [{0}]: {1}", alertEvent.ToString(), ex.MessageString());
                                alertEvent.IsProcessed = true;
                                await _alertEventRepository.UpdateAsync(alertEvent);
                                Thread.Sleep(Math.Max(1000, ApplicationViewModel.DeviceConfiguration.EMAIL_SEND_INTERVAL));
                            }
                            catch (Exception ex)
                            {
                                _log.Info(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "alertEvent [{0}] processed ERROR", alertEvent.ToString());
                                _log.Error(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), "Exception", "Error processing alertEvent [{0}]: {1}", alertEvent.ToString(), ex.MessageString());
                                alertEvent.IsProcessed = false;
                                await _alertEventRepository.UpdateAsync(alertEvent);
                                Thread.Sleep(Math.Max(1000, ApplicationViewModel.DeviceConfiguration.EMAIL_SEND_INTERVAL));
                            }
                            //SaveToDatabase(_depositorDBContext);
                        }
                    }
                    else
                        _log.Debug(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "Error: CanSendEmail = {0}, CanSendSMS = {1}", ApplicationViewModel.DeviceConfiguration.EMAIL_CAN_SEND, ApplicationViewModel.DeviceConfiguration.SMS_CAN_SEND);
                }
                catch (Exception ex)
                {
                    _log.Error(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), "Exception", "Error processing: {0}", ex.MessageString());
                }
                _log.Trace(nameof(DepositorCommunicationService), nameof(EmailSendingWorker_DoWork), nameof(EmailSendingWorker_DoWork), "while loop completed, sleeping...");
                Thread.Sleep(Math.Max(1000, ApplicationViewModel.DeviceConfiguration.ALERT_DB_POLL_INTERVAL));
            }
        }


        private EmailRequest GenerateEmail(
          Device device,
          AlertEmail alertEmail,
          ApplicationUser recipient)
        {

            _log.Debug(nameof(DepositorCommunicationService), nameof(GenerateEmail), nameof(GenerateEmail), "Generating MailMessage for AlertEmail:{0} and recipient: {1}", alertEmail.ToString(), recipient?.Email);
            if (alertEmail == null)
                throw new NullReferenceException("alertEmail cannot be null.");
            EmailRequest emailRequest = new EmailRequest
            {
                Message = new EmailMessage()
                {
                    HTMLContent = PersonaliseMessage(alertEmail.HtmlMessage, recipient),
                    ToAddresses = new List<EmailAddress>()
                                      {
                                        new EmailAddress()
                                        {
                                          Name = recipient?.FullName,
                                          Address = recipient?.Email
                                        }
                                      },
                    Subject = device.Name + ": " + alertEmail.Subject.Trim()
                },
                MessageDateTime = DateTime.Now,
                SessionID = alertEmail.Id.ToString(),
                MessageID = Guid.NewGuid().ToString(),
                AppID = _appId,
                AppName = _appName
            };
            return emailRequest;

        }

        private SMSRequest GenerateSms(
          AlertSMS alertSms,
          ApplicationUser recipient,
          string phone)
        {
            _log.Debug(nameof(DepositorCommunicationService), nameof(GenerateSms), nameof(GenerateSms), "Generating SMS for AlertSMS:{0} and recipient: {1}", alertSms.ToString(), recipient?.Phone);
            if (alertSms == null)
                throw new NullReferenceException("alertSMS cannot be null.");
            SMSRequest smsRequest = new SMSRequest
            {
                AppID = _appId,
                AppName = _appName,
                SMSMessage = new SMSMessage()
                {
                    MessageText = PersonaliseMessage(alertSms.Message, recipient),
                    ToContacts = new List<SMSContact>()
                          {
                            new SMSContact() { PhoneNumber = phone }
                          }
                },
                MessageDateTime = DateTime.Now,
                SessionID = alertSms.Id.ToString(),
                MessageID = Guid.NewGuid().ToString()
            };
            return smsRequest;

        }

        private string PersonaliseMessage(string message, ApplicationUser user)
        {
            _log.Debug(nameof(DepositorCommunicationService), nameof(PersonaliseMessage), nameof(PersonaliseMessage), "Personalising an email message for user:{0} ", user?.Email);
            string str = message;
            if (user != null)
                str = str.Replace("[user_fname]", user?.Fname).Replace("[user_lname]", user?.Lname).Replace("[user_username]", user?.Username).Replace("[user_email]", user?.Email).Replace("[user_phone]", user?.Phone).Replace("[user_role_name]", user?.Role.Name);
            return str;
        }

        private async void ProcessEmail(AlertEvent alertEvent)
        {
            if (!ApplicationViewModel.DeviceConfiguration.EMAIL_CAN_SEND)
                return;
            List<AlertEmail> list1 = await _alertEmailRepository.GetByAlertEventIdAsync(alertEvent.Id);
            Device device = await _deviceRepository.GetByIdAsync(alertEvent.DeviceId);
            if (device == null)
                throw new NullReferenceException("device cannot be null in EmailSendingWorker_DoWork()");
            Parallel.ForEach(list1, async alertEmail =>
            {
                _log.Info(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "processing alertEmail {0}", alertEmail.Id);
                try
                {
                    List<AlertEmailAttachment> alertEmailAttachmentList = new List<AlertEmailAttachment>();
                    List<EmailAttachment> alertEmailAttachments = new List<EmailAttachment>(5);
                    if (ApplicationViewModel.DeviceConfiguration.EMAIL_SEND_ATTACHMENT)
                        alertEmailAttachments = _alertEmailAttachmentRepository.GetAlertEmailAttachments(alertEmail.Id);
                    else
                        _log.Warning(nameof(DepositorCommunicationService), "Attachments Disabled", "GenerateEmail", "Email attachment sending is disabled in config");
                    _log.Debug(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "get the recipients");
                    //var depositorContextProcedures = new DepositorDBContextProcedures(depositorDbContext);
                    //var deviceUsersByDevice = depositorContextProcedures.GetDeviceUsersByDeviceAsync(device?.UserGroup).Result;

                    var deviceUsersByDevice = await _deviceRepository.GetByUserGroupAsync(device.UserGroup);
                    List<ApplicationUser> applicationUserList;
                    if (deviceUsersByDevice == null)
                    {
                        applicationUserList = null;
                    }
                    else
                    {
                        List<ApplicationUser> list2 = deviceUsersByDevice.ToList<ApplicationUser>();
                        if (list2 == null)
                        {
                            applicationUserList = null;
                        }
                        else
                        {
                            var source = list2.Where(deviceUser => (bool)deviceUser.DepositorEnabled && !string.IsNullOrEmpty(deviceUser.Email) && deviceUser.Role.AlertMessageRegistries.FirstOrDefault(alertMessageRegistryItem => alertMessageRegistryItem.AlertTypeId == alertEvent.AlertTypeId) != null);
                            applicationUserList = source != null ? source.ToList() : null;
                        }
                    }
                    IList<ApplicationUser> source1 = applicationUserList;
                    _log.Debug(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "found {0} recipients", source1.Count);
                    alertEmail.To = string.Join("|", source1 != null ? source1.Where(x => x?.Email != null).Select(x => x.Email).ToList() : (IEnumerable<string>)null);
                    alertEmail.To = string.IsNullOrWhiteSpace(alertEmail.To) ? null : alertEmail.To;
                    _log.Trace(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "send emails to each recipient");
                    Parallel.ForEach(source1, async recipient =>
                    {
                        try
                        {
                            EmailRequest emailTemplate = GenerateEmail(device, alertEmail, recipient);
                            emailTemplate.Message.Attachments = alertEmailAttachments;
                            _log.Debug(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "sending email {0} to {1}", alertEmail.ToString(), emailTemplate.Message.ToAddresses[0].Address);
                            await SendEmailAsync(emailTemplate);
                            _log.Info(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "sending email {0} to {1} SUCCESS", alertEmail.ToString(), emailTemplate.Message.ToAddresses[0].Address);
                            emailTemplate = null;
                        }
                        catch (TimeoutException ex)
                        {
                            _log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "TimeoutException", "TimeoutException sending alertEmail to [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                            throw;
                        }
                        catch (NullReferenceException ex)
                        {
                            _log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "NullReferenceException", "NullReferenceException sending alertEmail to [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                        }
                        catch (SmtpFailedRecipientsException ex)
                        {
                            _log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "SmtpFailedRecipientsException", "SmtpFailedRecipientsException sending alertEmail to [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                        }
                        catch (Exception ex)
                        {
                            _log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "Exception", "Exception sending alertEmail [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                        }
                        _log.Debug(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "processed recipient {0}", recipient?.Email);
                    });
                    _log.Info(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "alertEmail [{0}] processed SUCCESS", alertEmail.ToString());
                    alertEmail.Sent = true;
                    alertEmail.SendDate = new DateTime?(DateTime.Now);
                    alertEmail.SendError = false;
                    alertEmail.SendErrorMessage = "";
                }
                catch (Exception ex)
                {
                    _log.Info(nameof(DepositorCommunicationService), nameof(ProcessEmail), nameof(ProcessEmail), "alertEmail [{0}] processed ERROR", alertEmail.Id);
                    _log.Error(nameof(DepositorCommunicationService), nameof(ProcessEmail), "Exception", "Error processing alertEmail [{0}]: {1}", alertEmail.ToString(), ex.MessageString());
                    alertEmail.Sent = false;
                    alertEmail.SendError = true;
                    alertEmail.SendErrorMessage = ex.MessageString(new int?(200));
                }
            });
            //SaveToDatabase(depositorDbContext);
        }

        private async void ProcessSms(AlertEvent alertEvent)
        {
            if (!ApplicationViewModel.DeviceConfiguration.SMS_CAN_SEND)
                return;
            var alertSMs = await _alertSMSRepository.GetByAlertEventId(alertEvent.Id);
            Parallel.ForEach(alertSMs, async alertSms =>
            {
                // ISSUE: reference to a compiler-generated field
                Device device = await _deviceRepository.GetByIdAsync(alertEvent.DeviceId);
                if (device == null)
                    throw new NullReferenceException("device cannot be null in EmailSendingWorker_DoWork()");
                try
                {
                    _log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSms), nameof(ProcessSms), "get the recipients");
                    // var depositorContextProcedures = new DepositorDBContextProcedures(depositorDbContext);
                    //var deviceUsersByDevice = depositorContextProcedures.GetDeviceUsersByDeviceAsync(device?.UserGroup).Result;
                    var deviceUsersByDevice = await _deviceRepository.GetByUserGroupAsync(device.UserGroup);

                    List<ApplicationUser> applicationUserList;
                    if (deviceUsersByDevice == null)
                    {
                        applicationUserList = null;
                    }
                    else
                    {
                        List<ApplicationUser> list2 = deviceUsersByDevice.ToList<ApplicationUser>();
                        if (list2 == null)
                        {
                            applicationUserList = null;
                        }
                        else
                        {
                            IEnumerable<ApplicationUser> source = list2.Where(deviceUser => (bool)deviceUser.EmailEnabled && !string.IsNullOrEmpty(deviceUser.Email) && deviceUser.Role.AlertMessageRegistries.FirstOrDefault(alertMessageRegistryItem => alertMessageRegistryItem.AlertTypeId == alertEvent.AlertTypeId) != null);
                            applicationUserList = source != null ? source.ToList() : null;

                        }
                    }
                    IList<ApplicationUser> recipients = applicationUserList;
                    _log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSms), nameof(ProcessSms), "found {0} recipients", recipients.Count);
                    List<string> stringList1 = new List<string>(recipients.Count + 1);
                    if (!string.IsNullOrWhiteSpace(alertSms.To))
                        stringList1.Add(alertSms.To);
                    List<string> stringList2 = stringList1;
                    IList<ApplicationUser> source1 = recipients;
                    List<string> stringList3 = source1 != null ? source1.Where(x => x?.Phone != null).Select(x => x.Phone).ToList() : null;
                    stringList2.AddRange(stringList3);
                    alertSms.To = string.Join("|", stringList1);
                    AlertSMS alertSm = alertSms;
                    IList<ApplicationUser> source2 = recipients;
                    string str = string.Join("|", source2 != null ? source2.Where(x => x.PhoneEnabled && x.Phone != null).Select(x => x.Phone).ToList() : null);
                    alertSm.To = str;
                    alertSms.To = string.IsNullOrWhiteSpace(alertSms.To) ? null : alertSms.To;
                    _log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSms), nameof(ProcessSms), "send SMSes to each recipient");
                    Parallel.ForEach(stringList1, async phoneNumber =>
                    {
                        SMSRequest smsTemplate = GenerateSms(alertSms, recipients.FirstOrDefault(x => x.Phone.Equals(phoneNumber, StringComparison.Ordinal)), phoneNumber);
                        try
                        {
                            _log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSms), nameof(ProcessSms), "sending SMS {0} to {1}", alertSms.ToString(), smsTemplate.SMSMessage.ToContacts[0].PhoneNumber);
                            SMSResponse smsResponse = await SendSmsAsync(smsTemplate);
                            _log.Info(nameof(DepositorCommunicationService), nameof(ProcessSms), nameof(ProcessSms), "sending SMS {0} to {1} SUCCESS", alertSms.ToString(), smsTemplate.SMSMessage.ToContacts[0].PhoneNumber);
                        }
                        catch (TimeoutException ex)
                        {
                            _log.Error(nameof(DepositorCommunicationService), nameof(ProcessSms), "TimeoutException", "TimeoutException sending alertSMS [{0}] to {1} : {2}", alertSms.ToString(), smsTemplate.SMSMessage.ToContacts[0].PhoneNumber, ex.MessageString());
                        }
                        catch (NullReferenceException ex)
                        {
                            _log.Error(nameof(DepositorCommunicationService), nameof(ProcessSms), "NullReferenceException", "NullReferenceException sending alertSMS [{0}] to {1} : {2}", alertSms.ToString(), smsTemplate.SMSMessage.ToContacts[0].PhoneNumber, ex.MessageString());
                        }
                        catch (SmtpFailedRecipientsException ex)
                        {
                            _log.Error(nameof(DepositorCommunicationService), nameof(ProcessSms), "SmtpFailedRecipientsException", "SmtpFailedRecipientsException sending alertSMS [{0}] to {1} : {2}", alertSms.ToString(), smsTemplate.SMSMessage.ToContacts[0].PhoneNumber, ex.MessageString());
                        }
                        catch (Exception ex)
                        {
                            _log.Error(nameof(DepositorCommunicationService), nameof(ProcessSms), "Exception", "Exception sending alertSMS [{0}]: {1}", alertSms.ToString(), ex.MessageString());
                        }
                        _log.Debug(nameof(DepositorCommunicationService), nameof(ProcessSms), "processed recipient {0}", phoneNumber);
                        smsTemplate = null;
                    });
                    _log.Info(nameof(DepositorCommunicationService), nameof(ProcessSms), nameof(ProcessSms), "alertSMS [{0}] processed SUCCESS", alertSms.ToString());
                    alertSms.Sent = true;
                    alertSms.SendDate = new DateTime?(DateTime.Now);
                    alertSms.SendError = false;
                    alertSms.SendErrorMessage = "";
                    await _alertEventRepository.UpdateAsync(alertEvent);
                    //SaveToDatabase(depositorDbContext);
                }
                catch (Exception ex)
                {
                    _log.Info(nameof(DepositorCommunicationService), nameof(ProcessSms), nameof(ProcessSms), "alertSMS [{0}] processed ERROR", alertSms.Id);
                    _log.Error(nameof(DepositorCommunicationService), nameof(ProcessSms), "Exception", "Error processing alertSMS [{0}]: {1}", alertSms.ToString(), ex.MessageString());
                    alertSms.Sent = false;
                    alertSms.SendError = true;
                    alertSms.SendErrorMessage = ex.MessageString(new int?(200));
                    throw;
                }
            });
        }

        public static async Task SendEmailAsync(EmailRequest email)
        {
            CommunicationServiceClient communicationServiceClient = new CommunicationServiceClient(_commservUri, _appId, _appKey, null);
            try
            {
                EmailResponse emailResponse = await communicationServiceClient.SendEmailAsync(email);
            }
            catch (TimeoutException ex)
            {
                _log.Error(nameof(DepositorCommunicationService), "SendEmail", "TimeoutException", "Error sending email [{0}]: {1}", email.ToString(), ex.MessageString());
                throw;
            }
            catch (Exception ex)
            {
                _log.Error(nameof(DepositorCommunicationService), "SendEmail", "Exception", "Error sending email [{0}]: {1}", email.ToString(), ex.MessageString());
                throw;
            }
        }

        public static async Task<SMSResponse> SendSmsAsync(SMSRequest sms)
        {
            CommunicationServiceClient communicationServiceClient = new CommunicationServiceClient(_commservUri, _appId, _appKey, null);
            SMSResponse smsResponse;
            try
            {
                smsResponse = await communicationServiceClient.SendSMSAsync(sms);
            }
            catch (TimeoutException ex)
            {
                _log.Error(nameof(DepositorCommunicationService), "SendSMS", "TimeoutException", "Error sending SMS [{0}]: {1}", sms.ToString(), ex.MessageString());
                throw;
            }
            catch (Exception ex)
            {
                _log.Error(nameof(DepositorCommunicationService), "SendSMS", "Exception", "Error sending SMS [{0}]: {1}", sms.ToString(), ex.MessageString());
                throw;
            }
            return smsResponse;
        }

        public void Dispose()
        {
            _emailSendingWorker?.Dispose();
            _emailSendingWorker = null;
        }
    }
}

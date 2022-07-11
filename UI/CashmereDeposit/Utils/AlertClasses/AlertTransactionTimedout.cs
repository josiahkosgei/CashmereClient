﻿
// Type: CashmereDeposit.Utils.AlertClasses.AlertTransactionTimedout

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.Models;
using CashmereDeposit.ViewModels;

namespace CashmereDeposit.Utils.AlertClasses
{
    public class AlertTransactionTimedout : AlertBase
  {
    public const int ALERT_ID = 4008;
    private AppTransaction _transaction;

    public AlertTransactionTimedout(
      AppTransaction transaction,
      Device device,
      DateTime dateDetected)
      : base(device, dateDetected)
    {
      _transaction = transaction;
      using DepositorDBContext depositorDbContext = new DepositorDBContext();
      AlertType = depositorDbContext.AlertMessageTypes.FirstOrDefault(x => x.Id == 4008);
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
              entity.AlertSMSes.Add(sms);
          ApplicationViewModel.SaveToDatabase(DBContext);
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
      Tokens.Add("[date_detected]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
      Tokens.Add("[event_email_message]", GenerateHTMLMessageToken());
      Tokens.Add("[event_raw_message]", GenerateRawTextMessageToken());
      Tokens.Add("[event_sms_message]", GenerateSMSMessageToken());
    }

    protected new string GenerateHTMLMessageToken() => _transaction?.ToEmailString();

    protected new string GenerateRawTextMessageToken() => _transaction?.ToRawTextString();

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

    private new AlertEvent GetCorrespondingAlertEvent(DepositorDBContext DBContext) => throw new NotImplementedException();
  }
}
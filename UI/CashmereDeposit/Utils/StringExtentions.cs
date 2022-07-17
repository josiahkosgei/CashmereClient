
// Type: CashmereDeposit.Utils.StringExtentions




using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;

using CashmereDeposit.Models;
using CashmereDeposit.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit.Utils
{
    public static class StringExtentions
  {
    public static string StringReplace(
      this Device Device,
      string template,
      DeviceConfiguration DeviceConfiguration,
      bool isHTML = false)
    {
      List<string> stringList = new List<string>()
      {
        "{Device.name}",
        "{AlertMessageType.title}",
        "{AlertMessageType.description}"
      };
      return null;
    }

    public static string StringReplace(
      this AlertMessageType AlertMessageType,
      string template,
      DeviceConfiguration DeviceConfiguration,
      bool isHTML = false)
    {
      Parallel.ForEach(new List<(string, string)>()
      {
          ($"{{AlertMessageType_name}}", AlertMessageType?.Name),
          ($"{{AlertMessageType_title}}", AlertMessageType?.Title),
          ($"{{AlertMessageType_description}}", AlertMessageType?.Description)
      }, currentToken => template.Replace(currentToken.Item1, currentToken.Item2));
      return template;
    }

    public static string StringReplace(
      this AlertEvent AlertEvent,
      string template,
      DeviceConfiguration DeviceConfiguration,
      bool isHTML = false)
    {
        using DepositorDBContext depositorDbContext1 = new DepositorDBContext();
        AlertMessageType AlertMessageType = depositorDbContext1.AlertMessageTypes.FirstOrDefault(x => x.Id == AlertEvent.AlertTypeId);
        template = AlertMessageType != null ? AlertMessageType.StringReplace(template, DeviceConfiguration, isHTML) : null;
        DepositorDBContext depositorDbContext2 = new DepositorDBContext();
        string str1;
        if (depositorDbContext2 == null)
        {
            str1 = null;
        }
        else
        {
            DbSet<Device> devices = depositorDbContext2.Devices;
            if (devices == null)
            {
                str1 = null;
            }
            else
            {
                Device Device = devices.FirstOrDefault();
                str1 = Device != null ? Device.StringReplace(template, DeviceConfiguration, isHTML) : null;
            }
        }
        template = str1;
        List<(string, string)> valueTupleList = new List<(string, string)>();
        valueTupleList.Add(("{AlertEvent_alert_event_id}", AlertEvent?.AlertEventId.ToString().ToUpperInvariant()));
        valueTupleList.Add(("{AlertEvent_created}", AlertEvent?.Created.ToString(DeviceConfiguration.APPLICATION_DATE_FORMAT)));
        valueTupleList.Add(("{AlertEvent_date_detected}", AlertEvent?.DateDetected.ToString(DeviceConfiguration.APPLICATION_DATE_FORMAT)));
        AlertEvent alertEvent1 = AlertEvent;
        DateTime? dateResolved;
        int num;
        if (alertEvent1 == null)
        {
            num = 0;
        }
        else
        {
            dateResolved = alertEvent1.DateResolved;
            num = dateResolved.HasValue ? 1 : 0;
        }
        string str2;
        if (num == 0)
        {
            str2 = "";
        }
        else
        {
            AlertEvent alertEvent2 = AlertEvent;
            if (alertEvent2 == null)
            {
                str2 = null;
            }
            else
            {
                dateResolved = alertEvent2.DateResolved;
                str2 = dateResolved.Value.ToString(DeviceConfiguration.APPLICATION_DATE_FORMAT);
            }
        }
        valueTupleList.Add(("{AlertEvent_date_resolved}", str2));
        Parallel.ForEach(valueTupleList, currentToken => template.Replace(currentToken.Item1, currentToken.Item2));
        return template;
    }

    public static string CashmereReplace(this string s, ApplicationViewModel ApplicationViewModel) => s?.Replace("{transaction_limit_value}", ApplicationViewModel?.CurrentTransaction?.TransactionLimits?.OverdepositAmount.ToString("###,##0.00"))?.Replace("{transaction_underdeposit_amount}", ApplicationViewModel?.CurrentTransaction?.TransactionLimits?.UnderdepositAmount.ToString("###,##0.00"))?.Replace("{currency_code}", ApplicationViewModel?.CurrentTransaction?.CurrencyCode?.ToUpper())?.Replace("{bank_name}", ApplicationViewModel?.CurrentSession?.Device?.Branch?.Bank?.Name)?.Replace("{branch_name}", ApplicationViewModel?.CurrentSession?.Device?.Branch?.Name);
  }
}

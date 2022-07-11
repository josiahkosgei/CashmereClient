using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using CashmereUtil.Reporting.MSExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cashmere.API.CDM.Reporting.Uptime.Models.Messaging;
using Cashmere.API.CDM.Reporting.Uptime.Models.Reports;
using Cashmere.API.CDM.Reporting.UptimeDataAccess;
using Cashmere.API.CDM.Reporting.UptimeDataAccess.Models;

namespace Cashmere.API.CDM.UptimeReportLibrary
{
  public class UptimeReportGenerator : IUptimeReportGenerator
  {
    private IUptimeReportDataAccess DataAccess { get; set; }

    private IUptimeReportConfiguration Configuration { get; set; }

    public UptimeReportGenerator(
      IUptimeReportConfiguration config,
      IUptimeReportDataAccess dataAccess)
    {
      Configuration = config;
      DataAccess = dataAccess;
    }

    public async Task<UptimeReportResponse> GenerateUptimeReportAsync(UptimeReportRequest request)
    {
      UptimeReportResponse uptimeReportResponse1 = new UptimeReportResponse();
      UptimeReportResponse uptimeReportResponse2;
      try
      {
        Device Device = await DataAccess.GetDevice(request.Device) ?? throw new NullReferenceException(string.Format("Device '{0}' not found", request?.Device));
        DateTime fromDate = request.FromDate.Date;
        DateTime toDate = request.ToDate.Date;
        double totalDays = (toDate - fromDate).TotalDays;
        if (fromDate > toDate)
          throw new ArgumentException(string.Format("fromDate of {0:yyyy-MM-dd HH:mm:ss.fff ZZ} is after toDate of {1:yyyy-MM-dd HH:mm:ss.fff ZZ}", fromDate, toDate));
        List<UptimeMode> uptimeModesByRange = await DataAccess.GetUptimeModesByRange(request.Device, request.FromDate, request.ToDate);
        List<UptimeModeModel> UptimeModes = new List<UptimeModeModel>(uptimeModesByRange.Count + 10);
        foreach (UptimeMode uptimeMode in uptimeModesByRange)
        {
          uptimeMode.start_date = uptimeMode.start_date.Clamp(fromDate, toDate);
          DateTime dateTime1 = (uptimeMode.end_date ?? toDate).Clamp(fromDate, toDate);
          DateTime dateTime2;
          for (DateTime dateTime3 = uptimeMode.start_date; dateTime3 < dateTime1; dateTime3 = dateTime2)
          {
            dateTime2 = dateTime3.Date.AddDays(1.0);
            UptimeModes.Add(new UptimeModeModel()
            {
              device_mode = (UptimeModeType) uptimeMode.device_mode,
              start_date = dateTime3,
              end_date = dateTime1 < dateTime2 ? dateTime1 : dateTime2
            });
          }
        }
        IEnumerable<UptimeSummary> UptimeSummaries = UptimeModes.GroupBy(x => new { DayOfYear = x.start_date.DayOfYear
        }).Select(g => new UptimeSummary()
        {
          Start = new DateTime(fromDate.Year, 1, 1).AddDays((double) (g.Key.DayOfYear - 1)),
          End = new DateTime(fromDate.Year, 1, 1).AddDays((double) g.Key.DayOfYear),
          ACTIVE = TimeSpan.FromMilliseconds(g.Where<UptimeModeModel>((Func<UptimeModeModel, bool>) (f => f.device_mode == UptimeModeType.ACTIVE)).Sum<UptimeModeModel>((Func<UptimeModeModel, double>) (a => a.duration.TotalMilliseconds))),
          ADMIN = TimeSpan.FromMilliseconds(g.Where<UptimeModeModel>((Func<UptimeModeModel, bool>) (f => f.device_mode == UptimeModeType.ADMIN)).Sum<UptimeModeModel>((Func<UptimeModeModel, double>) (a => a.duration.TotalMilliseconds))),
          CIT = TimeSpan.FromMilliseconds(g.Where<UptimeModeModel>((Func<UptimeModeModel, bool>) (f => f.device_mode == UptimeModeType.CIT)).Sum<UptimeModeModel>((Func<UptimeModeModel, double>) (a => a.duration.TotalMilliseconds))),
          DEVICE_LOCKED = TimeSpan.FromMilliseconds(g.Where<UptimeModeModel>((Func<UptimeModeModel, bool>) (f => f.device_mode == UptimeModeType.DEVICE_LOCKED)).Sum<UptimeModeModel>((Func<UptimeModeModel, double>) (a => a.duration.TotalMilliseconds))),
          OUT_OF_ORDER = TimeSpan.FromMilliseconds(g.Where<UptimeModeModel>((Func<UptimeModeModel, bool>) (f => f.device_mode == UptimeModeType.OUT_OF_ORDER)).Sum<UptimeModeModel>((Func<UptimeModeModel, double>) (a => a.duration.TotalMilliseconds)))
        });
        List<UptimeComponentState> componentsByRange = await DataAccess.GetDeviceComponentsByRange(request.Device, request.FromDate, request.ToDate);
        List<ComponentModel> source = new List<ComponentModel>(componentsByRange.Count + 10);
        foreach (UptimeComponentState uptimeComponentState in componentsByRange)
        {
          uptimeComponentState.start_date = uptimeComponentState.start_date.Clamp(fromDate, toDate);
          DateTime dateTime1 = (uptimeComponentState.end_date ?? toDate).Clamp(fromDate, toDate);
          DateTime dateTime2;
          for (DateTime dateTime3 = uptimeComponentState.start_date; dateTime3 < dateTime1; dateTime3 = dateTime2)
          {
            dateTime2 = dateTime3.Date.AddDays(1.0);
            source.Add(new ComponentModel()
            {
              component_state = (CashmereDeviceState) uptimeComponentState.component_state,
              start_date = dateTime3,
              end_date = dateTime1 < dateTime2 ? dateTime1 : dateTime2
            });
          }
        }
        UptimeReport UptimeReport = new UptimeReport()
        {
          DeviceSummary = new DeviceSummary()
          {
            Device_Location = Device.device_location,
            Device_Name = Device.name,
            Device_Number = Device.device_number
          },
          ModeData = UptimeModes.OrderBy(x => x.start_date).ToArray(),
          ComponentData = source.OrderBy(x => x.start_date).ToArray(),
          UptimeReportSummary = new UptimeReportSummary()
          {
            StartDate = fromDate,
            EndDate = toDate,
            DeviceName = Device.name,
            DeviceLocation = Device.device_location,
            DeviceNumber = Device.device_number,
            UptimeSummary = UptimeSummaries.ToArray()
          }
        };
        string path = string.Format(request.UptimeReportPathFormat ?? Configuration.UptimeReportPathFormat, DateTime.Now, fromDate, toDate);
        string templatePath = request.UptimeReportTempatePath ?? Configuration.UptimeReportTempatePath;
        UptimeReportResponse uptimeReportResponse3 = new UptimeReportResponse
        {
            ReportBytes = ExcelManager.GenerateUptimeReportExcelAttachment(UptimeReport, templatePath, path),
            FileName = Path.GetFileName(path),
            SessionID = request.SessionID,
            MessageID = Guid.NewGuid().ToString().ToUpper(),
            AppID = request.AppID,
            AppName = request.AppName,
            RequestID = request.MessageID,
            MessageDateTime = DateTime.Now,
            IsSuccess = true,
            PublicErrorCode = null,
            PublicErrorMessage = null,
            ServerErrorCode = null,
            ServerErrorMessage = null
        };
        return uptimeReportResponse3;
      }
      catch (Exception ex)
      {
        UptimeReportResponse uptimeReportResponse3 = new UptimeReportResponse
        {
            SessionID = request.SessionID,
            MessageID = Guid.NewGuid().ToString().ToUpper(),
            AppID = request.AppID,
            AppName = request.AppName,
            RequestID = request.MessageID,
            MessageDateTime = DateTime.Now,
            IsSuccess = false,
            PublicErrorCode = "500",
            PublicErrorMessage = "Server error. Contact Administrator",
            ServerErrorCode = "500",
            ServerErrorMessage = ex.MessageString()
        };
        uptimeReportResponse2 = uptimeReportResponse3;
      }
      return uptimeReportResponse2;
    }

  }
}

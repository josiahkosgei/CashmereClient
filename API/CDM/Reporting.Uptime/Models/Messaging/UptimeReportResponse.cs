using Cashmere.API.CDM.Reporting.Uptime.Models.Reports;
using Cashmere.API.Messaging;

namespace Cashmere.API.CDM.Reporting.Uptime.Models.Messaging
{
  public class UptimeReportResponse : APIResponseBase
  {
    public UptimeReport UptimeReport { get; set; }

    public byte[] ReportBytes { get; set; }

    public string FileName { get; set; }
  }
}

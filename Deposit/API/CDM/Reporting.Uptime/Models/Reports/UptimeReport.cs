
namespace Cashmere.API.CDM.Reporting.Uptime.Models.Reports
{
    public class UptimeReport
    {
        public DeviceSummary DeviceSummary { get; set; }

        public UptimeReportSummary UptimeReportSummary { get; set; }

        public UptimeModeModel[] ModeData { get; set; }

        public ComponentModel[] ComponentData { get; set; }
    }
}

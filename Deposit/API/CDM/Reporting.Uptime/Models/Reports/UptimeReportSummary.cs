
namespace Cashmere.API.CDM.Reporting.Uptime.Models.Reports
{
    public class UptimeReportSummary
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Days => (EndDate - StartDate).TotalDays;

        public string DeviceName { get; set; }

        public string DeviceLocation { get; set; }

        public string DeviceNumber { get; set; }

        public UptimeSummary[] UptimeSummary { get; set; }
    }
}

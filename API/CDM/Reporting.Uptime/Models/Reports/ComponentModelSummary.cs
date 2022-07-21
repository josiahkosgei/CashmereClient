
namespace Cashmere.API.CDM.Reporting.Uptime.Models.Reports
{
    public class ComponentModelSummary
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public TimeSpan ACTIVE { get; set; }

        public TimeSpan ADMIN { get; set; }

        public TimeSpan CIT { get; set; }

        public TimeSpan OUT_OF_ORDER { get; set; }

        public TimeSpan DEVICE_LOCKED { get; set; }
    }
}

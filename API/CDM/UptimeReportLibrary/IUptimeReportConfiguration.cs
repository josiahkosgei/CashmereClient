
namespace Cashmere.API.CDM.UptimeReportLibrary
{
    public interface IUptimeReportConfiguration
    {
        string UptimeReportPathFormat { get; set; }

        string UptimeReportTempatePath { get; set; }

        string ConnectionString { get; set; }
    }
}

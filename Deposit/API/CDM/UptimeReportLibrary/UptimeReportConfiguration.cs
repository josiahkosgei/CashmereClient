namespace Cashmere.API.CDM.UptimeReportLibrary
{
    public class UptimeReportConfiguration : IUptimeReportConfiguration
    {
        public string UptimeReportPathFormat { get; set; }

        public string UptimeReportTempatePath { get; set; }

        public string ConnectionString { get; set; }

        public UptimeReportConfiguration(string connectionString) => ConnectionString = ConnectionString;

        public UptimeReportConfiguration()
        {
        }

        public UptimeReportConfiguration ConfigureConnectionString(
          string connectionString)
        {
            ConnectionString = connectionString;
            return this;
        }
    }
}

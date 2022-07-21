using Cashmere.Library.Standard.Logging;
using System.Threading.Tasks;
using Cashmere.API.CDM.Reporting.Uptime.Models.Messaging;

namespace Cashmere.API.CDM.Reporting.Uptime
{
    public interface IUptimeReportController
    {
        ICashmereAPILogger Log { get; set; }

        Task<UptimeReportResponse> GetUptimeReportAsync(
          UptimeReportRequest request);
    }
}

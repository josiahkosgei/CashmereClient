

using System.Threading.Tasks;
using Cashmere.API.CDM.Reporting.Uptime.Models.Messaging;

namespace Cashmere.API.CDM.UptimeReportLibrary
{
    public interface IUptimeReportGenerator
    {
        Task<UptimeReportResponse> GenerateUptimeReportAsync(UptimeReportRequest request);
    }
}

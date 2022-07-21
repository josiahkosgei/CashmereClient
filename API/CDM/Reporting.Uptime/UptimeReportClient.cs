using Cashmere.API.CDM.Reporting.Uptime.Models.Messaging;
using Cashmere.API.CDMMessaging;
using Cashmere.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;

namespace Cashmere.API.CDM.Reporting.Uptime
{
    public class UptimeReportClient : CDM_APIClient, IUptimeReportController
    {
        public ICashmereAPILogger Log { get; set; }

        public UptimeReportClient(
          string apiBaseAddress,
          Guid AppID,
          byte[] appKey,
          IConfiguration configuration)
          : base(new CashmereAPILogger(nameof(UptimeReportClient), configuration), apiBaseAddress, AppID, appKey, configuration)
        {
        }

        public async Task<UptimeReportResponse> GetUptimeReportAsync(UptimeReportRequest request) => await SendAsync<UptimeReportResponse>("api/UptimeReport/Generate", request);
    }
}

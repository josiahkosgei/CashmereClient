using Cashmere.API.CDM.UptimeReportLibrary;
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Threading.Tasks;
using Cashmere.API.CDM.Reporting.Uptime;
using Cashmere.API.CDM.Reporting.Uptime.Models.Messaging;

namespace Cashmere.API.CDM.ReportingService.Controllers
{
  public class UptimeReportController : IUptimeReportController
  {
    public UptimeReportController(
      IUptimeReportConfiguration uptimeReportConfiguration,
      ICashmereAPILogger log,
      IUptimeReportGenerator uptimeReportGenerator)
    {
      Log = log ?? throw new ArgumentNullException(nameof (log));
      UptimeReportConfiguration = uptimeReportConfiguration;
      UptimeReportGenerator = uptimeReportGenerator ?? throw new ArgumentNullException(nameof (uptimeReportGenerator));
    }

    private IUptimeReportConfiguration UptimeReportConfiguration { get; set; }

    public ICashmereAPILogger Log { get; set; }

    public IUptimeReportGenerator UptimeReportGenerator { get; set; }

    public async Task<UptimeReportResponse> GetUptimeReportAsync(
      UptimeReportRequest request)
    {
      UptimeReportResponse uptimeReportResponse1 = new UptimeReportResponse();
      UptimeReportResponse uptimeReportResponse2;
      try
      {
        uptimeReportResponse2 = await UptimeReportGenerator?.GenerateUptimeReportAsync(request);
      }
      catch (Exception ex)
      {
        UptimeReportResponse uptimeReportResponse3 = new UptimeReportResponse();
        uptimeReportResponse3.AppID = request.AppID;
        uptimeReportResponse3.AppName = request.AppName;
        uptimeReportResponse3.RequestID = request.MessageID;
        uptimeReportResponse3.SessionID = request.SessionID;
        uptimeReportResponse3.MessageDateTime = DateTime.Now;
        uptimeReportResponse3.MessageID = Guid.NewGuid().ToString();
        uptimeReportResponse3.IsSuccess = false;
        uptimeReportResponse3.PublicErrorCode = "500";
        uptimeReportResponse3.PublicErrorMessage = "Server error. Contact Administrator.";
        uptimeReportResponse3.ServerErrorCode = string.Format("{0:#}", 500);
        uptimeReportResponse3.ServerErrorMessage = ex.MessageString();
        uptimeReportResponse2 = uptimeReportResponse3;
      }
      return uptimeReportResponse2;
    }
  }
}

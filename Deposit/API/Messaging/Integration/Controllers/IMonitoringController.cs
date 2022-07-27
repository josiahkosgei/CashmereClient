// IMonitoringController


using Cashmere.API.Messaging.Integration.ServerPing;

namespace Cashmere.API.Messaging.Integration.Controllers
{
    public interface IMonitoringController
    {
        Task<IntegrationServerPingResponse> ServerPingAsync(
          IntegrationServerPingRequest request);
    }
}

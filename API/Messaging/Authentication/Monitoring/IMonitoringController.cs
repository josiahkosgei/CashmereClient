using System.Threading.Tasks;

namespace Cashmere.API.Messaging.Authentication.Monitoring
{
    public interface IMonitoringController
    {
        Task<AuthenticationServerPingResponse> ServerPingAsync(
          AuthenticationServerPingRequest request);
    }
}

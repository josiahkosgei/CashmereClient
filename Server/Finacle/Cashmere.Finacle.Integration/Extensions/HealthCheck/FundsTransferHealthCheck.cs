using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace Cashmere.Finacle.Integration.Extensions.HealthCheck
{
    public class FundsTransferHealthCheck : IHealthCheck
    {
        private readonly string _url;

        public FundsTransferHealthCheck(string BSUrl = "192.168.0.180")
        {
            _url = BSUrl;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {


            var result = await PspingUtility.CustomPingAsync(_url);
            if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
                return HealthCheckResult.Healthy($"returned a {result.Status} status Code. Up and Running");

            return HealthCheckResult.Unhealthy($"endpoint is down: FundsTransfer 4.0 returned a {result.Status} status Code");

        }
    }
}
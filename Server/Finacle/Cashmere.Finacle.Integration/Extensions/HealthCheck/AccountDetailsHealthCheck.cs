using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Cashmere.Finacle.Integration.Extensions.HealthCheck
{
    public class AccountDetailsHealthCheck : IHealthCheck
    {
        private readonly string _url;

        public AccountDetailsHealthCheck(string BSUrl = "192.168.0.180")
        {
            _url = BSUrl;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {


            var result = await PspingUtility.CustomPingAsync(_url);
            if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
                return HealthCheckResult.Healthy($"returned a {result.Status} status Code. Up and Running");

            return HealthCheckResult.Unhealthy($"endpoint is down: AccountDetails 3.0 returned a {result.Status} status Code");

        }
    }
    public class CDMHealthCheck : IHealthCheck
    {
        private readonly string _url;

        public CDMHealthCheck(string BSUrl = "192.168.0.180")
        {
            _url = BSUrl;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {


            var result = await PspingUtility.CustomPingAsync(_url);
            if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
                return HealthCheckResult.Healthy($"returned a {result.Status} status Code. Up and Running");

            return HealthCheckResult.Unhealthy($"endpoint is down: CMD 6.3 returned a {result.Status} status Code");

        }
    }
}
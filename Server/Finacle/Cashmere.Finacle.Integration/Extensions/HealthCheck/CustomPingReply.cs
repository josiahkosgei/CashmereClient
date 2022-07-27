using System.Net.NetworkInformation;

namespace Cashmere.Finacle.Integration.Extensions.HealthCheck
{
    internal class CustomPingReply
    {
        public IPStatus Status { get; set; }
    }
}

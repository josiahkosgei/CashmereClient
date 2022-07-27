using System.Collections.Generic;

namespace Cashmere.API.Messaging.Authentication.ActiveDirectory
{
    public interface IActiveDirectoryConfiguration
    {
        List<ADServer> ADServers { get; set; }

        int SendInterval { get; set; }

        int SendRetryLimit { get; set; }

        int Timeout { get; set; }

        bool UseSSL { get; set; }

        bool IgnoreCert { get; set; }

        string Domain { get; set; }

        string BaseDN { get; }
    }
}

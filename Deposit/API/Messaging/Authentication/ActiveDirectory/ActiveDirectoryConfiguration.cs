
using System.Collections.Generic;

namespace Cashmere.API.Messaging.Authentication.ActiveDirectory
{
    public class ActiveDirectoryConfiguration : IActiveDirectoryConfiguration
    {
        public List<ADServer> ADServers { get; set; }

        public int SendInterval { get; set; }

        public int SendRetryLimit { get; set; }

        public int Timeout { get; set; }

        public bool UseSSL { get; set; }

        public bool IgnoreCert { get; set; }

        public string Domain { get; set; }

        public string BaseDN { get; set; }
    }
}

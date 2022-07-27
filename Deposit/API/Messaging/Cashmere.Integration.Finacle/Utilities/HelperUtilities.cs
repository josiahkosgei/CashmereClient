using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using BSAccountDetailsServiceReference;
using BSAccountServiceReference;

namespace Cashmere.Integration.Finacle.Utilities
{
    public static class HelperUtilities
    {
        private static Binding _binding;
        private static EndpointAddress _remoteAddress;

        //private static string NAVPassword = Convert.ToString(ConfigurationManager.AppSettings["NAVPassword"]);
        //private static string NAVUsername = Convert.ToString(ConfigurationManager.AppSettings["NAVUsername"]);
        //private static string NAVUri = Convert.ToString(ConfigurationManager.AppSettings["NAVUri"]);
        //private static string NAVDomainUser = Convert.ToString(ConfigurationManager.AppSettings["NAVDomainUser"]);
        public static void Init(Binding binding, EndpointAddress remoteAddress)
        {
            _binding = binding;
            _remoteAddress = remoteAddress;
        }
        public static BSGetAccountDetailsClient IntegrationBsGetAccountDetailsClientService
        {
            get
            {
                BSGetAccountDetailsClient nav = null;


                try
                {
                    nav = new BSGetAccountDetailsClient(_binding, _remoteAddress);

                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }

                return nav;
            }
        }
        public static BSAccountClient IntegrationBSAccountClientService
        {
            get
            {
                BSAccountClient nav = null;


                try
                {
                    nav = new BSAccountClient(_binding, _remoteAddress);

                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }

                return nav;
            }
        }
    }
}

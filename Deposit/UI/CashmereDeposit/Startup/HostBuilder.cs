using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace CashmereDeposit.Startup
{
    public static class HostBuilder
    {
        private static IWebHost host;

        public static async Task Start()
        {
            if (host == null)
            {
                var ip = System.Net.IPAddress.Parse("127.0.0.1");
                var port = 9000;

                host = new WebHostBuilder()
                    .UseKestrel(options =>
                    {
                        options.Listen(ip, port);
                    })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    //.UseIISIntegration()
                    .UseStartup<HostStartup>()
                    .Build();

                await host.RunAsync();
            }
        }

    }
}

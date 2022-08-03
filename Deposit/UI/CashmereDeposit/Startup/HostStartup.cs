using System.Web.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CashmereDeposit.Startup
{
    public class HostStartup
    {
        public IConfiguration Configuration { get; }

        public HostStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(MvcOptions =>
            {
                MvcOptions.EnableEndpointRouting = false;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, System.IServiceProvider serviceProvider)
        {

            app.UseMvc(builder =>
            {
                builder.MapRoute("DefaultApi", "api/{controller}/{id}", new
                {
                    id = RouteParameter.Optional
                });
            });
        }
    }

}

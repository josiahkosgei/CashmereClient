using Cashmere.Finacle.Integration.CQRS.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;

namespace Cashmere.Finacle.Integration.Extensions.HealthCheck
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructureHealthCheck(this IServiceCollection services, IConfiguration Configuration)
        {
            var provider = services.BuildServiceProvider();
            var _globalSettings = provider.GetRequiredService<IOptionsMonitor<SOAServerConfiguration>>()?.CurrentValue;

            string connectionString = Configuration.GetConnectionString("Default");

            services.AddHealthChecks()
                .AddCheck<FundsTransferHealthCheck>("fundstransfer", tags: new string[] { "funds", "funds transfer", "4.0" })
                .AddCheck<AccountDetailsHealthCheck>("accountdetails", tags: new string[] { "account", "account details", "3.0" })
                .AddCheck<AccountDetailsHealthCheck>("cdm", tags: new string[] { "cdm", "cashmere deposit machine", "6.0" })
                .AddSqlServer(connectionString, "SELECT 1;", "sqlserver", tags: new string[] { "db", "sql", "sqlserver" });


            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.AddHealthCheckEndpoint("sqlserver", $"{_globalSettings.APPLICATION_PATH.AppendAsURL("healthz-db")}");
                setup.AddHealthCheckEndpoint("fundstransfer", $"{_globalSettings.APPLICATION_PATH.AppendAsURL("healthz-fundstransfer")}");
                setup.AddHealthCheckEndpoint("accountdetails", $"{_globalSettings.APPLICATION_PATH.AppendAsURL("healthz-accountdetails")}");
                setup.AddHealthCheckEndpoint("cdm", $"{_globalSettings.APPLICATION_PATH.AppendAsURL("healthz-cdm")}");
                setup.SetEvaluationTimeInSeconds(60); //Configures the UI to poll for healthchecks updates every 60 seconds
                setup.SetApiMaxActiveRequests(1); //Only one active request will be executed at a time.
                setup.MaximumHistoryEntriesPerEndpoint(50);
            }).AddInMemoryStorage();


            return services;
        }

    }
}

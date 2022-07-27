using Cashmere.Finacle.Integration.CQRS.Commands.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.Interfaces;
using Cashmere.Finacle.Integration.CQRS.Mappers;
using MediatR;
using AutoMapper;
using System.Reflection;
using Serilog;
using Serilog.Events;
using Cashmere.Finacle.Integration.CQRS.Helpers;
using HealthChecks.UI.Core;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Cashmere.Finacle.Integration.Extensions;
using Microsoft.OpenApi.Models;
using Cashmere.Finacle.Integration.Extensions.HealthCheck;
using Cashmere.Finacle.Integration.CQRS.DataAccessLayer.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string timestamp = DateTime.Now.ToString("yyyy-MM-dd");
var seriLogger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
               System.IO.Path.Combine(@"C:\\CashmereDeposit", "Transactions", "FinacleIntegration", "API", "6.0", $"finacle-integration-{timestamp}.txt"),
               rollingInterval: RollingInterval.Infinite,
               fileSizeLimitBytes: 10 * 1024 * 1024,
               retainedFileCountLimit: 2,
               rollOnFileSizeLimit: true,
               shared: true,
               flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true);

//builder.Configuration.GetSection("SOAServerConfiguration").Get<SOAServerConfiguration>();
builder.Services.Configure<SOAServerConfiguration>(configuration.GetSection("SOAServerConfiguration"));
builder.Services.AddTransient<DepositorServerContext>();
builder.Services.AddControllers();
builder.Services.AddInfrastructureHealthCheck(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Cashmere Finacle Integration Api", Version = "v1" });
                swaggerOptions.DocumentFilter<HealthCheckEndpointDocumentFilter>();
            });
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Logging.ClearProviders();

builder.Logging.AddSerilog(seriLogger);
//builder.Host.UseSerilog(seriLogger);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
    });
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/healthz-fundstransfer", new HealthCheckOptions()
                {
                    Predicate = _ => _.Tags.Contains("funds"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/healthz-accountdetails", new HealthCheckOptions()
                {
                    Predicate = _ => _.Tags.Contains("account"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/healthz-db", new HealthCheckOptions()
                {
                    Predicate = _ => _.Tags.Contains("db"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/healthz-cdm", new HealthCheckOptions()
                {
                    Predicate = _ => _.Tags.Contains("cdm"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI();
            });
app.MigrateDatabase<DepositorServerContext>((context, services) => { }).Run();

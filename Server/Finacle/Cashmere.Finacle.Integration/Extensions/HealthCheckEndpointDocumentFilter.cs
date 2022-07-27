using HealthChecks.UI.Core;
using HealthChecks.UI.Core.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using static System.Text.Json.JsonNamingPolicy;

namespace Cashmere.Finacle.Integration.Extensions
{
    public class HealthCheckEndpointDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly global::HealthChecks.UI.Configuration.Options _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public HealthCheckEndpointDocumentFilter(IOptions<global::HealthChecks.UI.Configuration.Options> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var pathItem = new OpenApiPathItem
            {
                Operations = new Dictionary<OperationType, OpenApiOperation>
                {
                    [OperationType.Get] = new OpenApiOperation
                    {
                        Description = "Returns all the health states used by this Microservice",
                        Tags =
                        {
                            new OpenApiTag
                            {
                                Name = "HealthCheck"
                            }
                        },
                        Responses =
                        {
                            [StatusCodes.Status200OK.ToString()] = new OpenApiResponse
                            {
                                Description = "API is healthy",
                                Content =
                                {
                                    ["application/json"] = new OpenApiMediaType
                                    {
                                        Schema = new OpenApiSchema
                                        {
                                            Reference = new OpenApiReference
                                            {
                                                Id = nameof(HealthCheckExecution),
                                                Type = ReferenceType.Schema,
                                            }
                                        }
                                    }
                                }
                            },
                            [StatusCodes.Status503ServiceUnavailable.ToString()] = new OpenApiResponse
                            {
                                Description = "API is not healthy"
                            }
                        }
                    }
                }
            };

            var healthCheckSchema = new OpenApiSchema
            {
                Type = "object",
                Properties =
                {
                    [CamelCase.ConvertName(nameof(HealthCheckExecution.Id))] = new OpenApiSchema
                    {
                        Type = "integer",
                        Format = "int32"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecution.Status))] = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecution.OnStateFrom))] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "date-time"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecution.LastExecuted))] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "date-time"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecution.Uri))] = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecution.Name))] = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecution.DiscoveryService))] = new OpenApiSchema
                    {
                        Type = "string",
                        Nullable = true
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecution.Entries))] = new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Id = nameof(HealthCheckExecutionEntry),
                                Type = ReferenceType.Schema,
                            }
                        }
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecution.History))] = new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Id = nameof(HealthCheckExecutionHistory),
                                Type = ReferenceType.Schema,
                            }
                        }
                    }
                }
            };

            var healthCheckEntrySchema = new OpenApiSchema
            {
                Type = "object",

                Properties =
                {
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionEntry.Id))] = new OpenApiSchema
                    {
                        Type = "integer",
                        Format = "int32"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionEntry.Name))] = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionEntry.Status))] = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Id = nameof(UIHealthStatus),
                            Type = ReferenceType.Schema,
                        }
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionEntry.Description))] = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionEntry.Duration))] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "[-][d'.']hh':'mm':'ss['.'fffffff]"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionEntry.Tags))] = new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema
                        {
                            Type = "string"
                        }
                    },
                }
            };

            var healthCheckHistorySchema = new OpenApiSchema
            {
                Type = "object",

                Properties =
                {
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionHistory.Id))] = new OpenApiSchema
                    {
                        Type = "integer",
                        Format = "int32"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionHistory.Name))] = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionHistory.Description))] = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionHistory.Status))] = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Id = nameof(UIHealthStatus),
                            Type = ReferenceType.Schema,
                        }
                    },
                    [CamelCase.ConvertName(nameof(HealthCheckExecutionHistory.On))] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "date-time"
                    },
                }
            };

            var uiHealthStatusSchema = new OpenApiSchema
            {
                Type = "string",

                Enum =
                {
                    new OpenApiString(UIHealthStatus.Healthy.ToString()),
                    new OpenApiString(UIHealthStatus.Unhealthy.ToString()),
                    new OpenApiString(UIHealthStatus.Degraded.ToString())
                }
            };

            swaggerDoc.Paths.Add(_options.ApiPath, pathItem);
            swaggerDoc.Components.Schemas.Add(nameof(HealthCheckExecution), healthCheckSchema);
            swaggerDoc.Components.Schemas.Add(nameof(HealthCheckExecutionEntry), healthCheckEntrySchema);
            swaggerDoc.Components.Schemas.Add(nameof(HealthCheckExecutionHistory), healthCheckHistorySchema);
            swaggerDoc.Components.Schemas.Add(nameof(UIHealthStatus), uiHealthStatusSchema);
        }
    }
}

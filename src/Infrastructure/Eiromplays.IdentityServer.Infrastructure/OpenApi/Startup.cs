using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema.Generation.TypeMappers;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation;
using NSwag.Generation.Processors.Security;
using ZymLabs.NSwag.FluentValidation;

namespace Eiromplays.IdentityServer.Infrastructure.OpenApi;

internal static class Startup
{
    internal static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();

        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        if (!settings.Enable) return services;

        services
            .AddSwaggerDoc(
                settings: s =>
            {
                s.PostProcess = doc =>
                    {
                        doc.Info.Title = settings.Title;
                        doc.Info.Version = settings.Version;
                        doc.Info.Description = settings.Description;
                        doc.Info.Contact = new OpenApiContact
                        {
                            Name = settings.ContactName,
                            Email = settings.ContactEmail,
                            Url = settings.ContactUrl
                        };
                        doc.Info.License = new OpenApiLicense
                        {
                            Name = settings.LicenseName,
                            Url = settings.LicenseUrl
                        };
                    };

                s.AddOpenApiDocumentationSecurity(config);

                s.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
                s.OperationProcessors.Add(new SwaggerGlobalAuthProcessor());

                s.TypeMappers.Add(new PrimitiveTypeMapper(typeof(TimeSpan), schema =>
                    {
                        schema.Type = NJsonSchema.JsonObjectType.String;
                        schema.IsNullableRaw = true;
                        schema.Pattern = @"^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:.(\d{1,9}))?)?$";
                        schema.Example = "02:00:00";
                    }));

                s.OperationProcessors.Add(new SwaggerHeaderAttributeProcessor());

                /*var fluentValidationSchemaProcessor = services.BuildServiceProvider().CreateScope().ServiceProvider.GetService<FluentValidationSchemaProcessor>();
                Console.WriteLine(fluentValidationSchemaProcessor != null);
                s.SchemaProcessors.Add(fluentValidationSchemaProcessor);*/
            },
                maxEndpointVersion: 1);

        // services.AddScoped<FluentValidationSchemaProcessor>();

        return services;
    }

    internal static OpenApiDocumentGeneratorSettings AddOpenApiDocumentationSecurity(
        this OpenApiDocumentGeneratorSettings settings,
        IConfiguration config)
    {
        settings.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.OAuth2,
            Flow = OpenApiOAuth2Flow.AccessCode,
            Description = "OAuth2.0 Auth Code with PKCE",
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = config["SecuritySettings:Swagger:AuthorizationUrl"],
                    TokenUrl = config["SecuritySettings:Swagger:TokenUrl"],
                    Scopes = new Dictionary<string, string>
                    {
                        { config["SecuritySettings:Swagger:ApiScope"] ?? string.Empty, "access the api" }
                    }
                }
            }
        });
        return settings;
    }

    internal static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();

        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        if (!settings.Enable) return app;

        app.UseOpenApi()
            .UseSwaggerUi3(options =>
            {
                var swaggerUiSettings = config.GetSection(nameof(SwaggerUiSettings)).Get<SwaggerUiSettings>();
                options.DefaultModelsExpandDepth = -1;
                options.DocExpansion = "none";
                options.TagsSorter = "alpha";

                if (swaggerUiSettings?.OAuth2Client is not null)
                    options.OAuth2Client = swaggerUiSettings.OAuth2Client;

                options.AdditionalSettings.Add("persistAuthorization", "true");
            });

        return app;
    }
}
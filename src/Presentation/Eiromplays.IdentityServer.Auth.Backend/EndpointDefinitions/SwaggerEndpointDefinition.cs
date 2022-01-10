using Microsoft.OpenApi.Models;

namespace Eiromplays.IdentityServer.EndpointDefinitions;

public class SwaggerEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eiromplays.IdentityServer v1"));
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eiromplays.IdentityServer", Version = "v1" });
        });
    }
}
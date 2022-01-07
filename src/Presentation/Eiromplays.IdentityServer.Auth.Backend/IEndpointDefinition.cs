namespace Eiromplays.IdentityServer.Auth.Backend;

public interface IEndpointDefinition
{
    void DefineServices(IServiceCollection services);

    void DefineEndpoints(WebApplication app);
}
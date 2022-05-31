using Eiromplays.IdentityServer.Application.Identity.ApiResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiResources.Create;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IApiResourceService _apiResourceService;
    
    public Endpoint(IApiResourceService apiResourceService)
    {
        _apiResourceService = apiResourceService;
    }

    public override void Configure()
    {
        Post("/api-resources");
        Summary(s =>
        {
            s.Summary = "Creates a new ApiResources.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Create, EIAResource.ApiResources));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _apiResourceService.CreateAsync(req.Data, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}
using Eiromplays.IdentityServer.Application.Identity.ApiResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.ApiResources.Create;

public class Endpoint : Endpoint<CreateApiResourceRequest, Models.Response>
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
        ScopedValidator();
    }

    public override async Task HandleAsync(CreateApiResourceRequest req, CancellationToken ct)
    {
        Response.Message = await _apiResourceService.CreateAsync(req, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}
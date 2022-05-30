using Eiromplays.IdentityServer.Application.Identity.Clients;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Clients.GetByClientById;

public class Endpoint : Endpoint<Models.Request, ClientDto>
{
    private readonly IClientService _clientService;
    
    public Endpoint(IClientService clientService)
    {
        _clientService = clientService;
    }

    public override void Configure()
    {
        Get("/clients/{Id}");
        Summary(s =>
        {
            s.Summary = "Get client details.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.View, EIAResource.Clients));
    }

    public override async Task HandleAsync(Models.Request request, CancellationToken ct)
    {
        Response = await _clientService.GetAsync(request.Id, ct);

        await SendAsync(Response, cancellation: ct);
    }
}
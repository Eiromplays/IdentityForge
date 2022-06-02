using Eiromplays.IdentityServer.Application.Identity.Clients;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Clients.UpdateClient;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IClientService _clientService;
    
    public Endpoint(IClientService clientService)
    {
        _clientService = clientService;
    }

    public override void Configure()
    {
        Put("/clients/{Id}");
        Summary(s =>
        {
            s.Summary = "Update a client.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Update, EIAResource.Clients));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        await _clientService.UpdateAsync(req.Data, req.Id, ct);
        
        await SendNoContentAsync(cancellation: ct);
    }
}
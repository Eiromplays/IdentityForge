using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Error;

namespace Eiromplays.IdentityServer.Endpoints.v1.Error;

public class GetErrorEndpoint : Endpoint<GetErrorRequest, ErrorMessage>
{
    private readonly IIdentityServerInteractionService _interaction;

    public GetErrorEndpoint(IIdentityServerInteractionService interaction)
    {
        _interaction = interaction;
    }

    public override void Configure()
    {
        Get("/errors");
        Version(1);
        Summary(s =>
        {
            s.Summary = "Get error information";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetErrorRequest req, CancellationToken ct)
    {
        Response = await _interaction.GetErrorContextAsync(req.ErrorId);

        await SendOkAsync(Response, ct);
    }
}
namespace Eiromplays.IdentityServer.Application.Identity.Auth.Requests.Error;

public class GetErrorRequest
{
    [QueryParam]
    public string ErrorId { get; set; } = default!;
}
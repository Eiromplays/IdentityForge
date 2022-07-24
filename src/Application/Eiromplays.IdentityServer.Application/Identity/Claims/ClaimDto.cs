namespace Eiromplays.IdentityServer.Application.Identity.Claims;

public class ClaimDto
{
    public string Type { get; set; } = default!;
    public string Value { get; set; } = default!;

    public string ValueType { get; set; } = default!;
    public string Issuer { get; set; } = default!;
}
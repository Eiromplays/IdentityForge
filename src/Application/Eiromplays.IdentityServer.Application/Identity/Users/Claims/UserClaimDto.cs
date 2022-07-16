namespace Eiromplays.IdentityServer.Application.Identity.Users.Claims;

public class UserClaimDto
{
    public string? Type { get; set; }
    public string? Value { get; set; }
    public string? ValueType { get; set; }
    public string? Issuer { get; set; }
}
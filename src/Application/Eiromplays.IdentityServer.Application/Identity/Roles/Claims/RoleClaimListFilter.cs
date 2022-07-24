namespace Eiromplays.IdentityServer.Application.Identity.Roles.Claims;

public class RoleClaimListFilter : PaginationFilter
{
    public string RoleId { get; set; } = default!;
}
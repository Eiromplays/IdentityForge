namespace Eiromplays.IdentityServer.Application.Identity.Users.Claims;

public class UserClaimListFilter : PaginationFilter
{
    public string UserId { get; set; } = default!;
}
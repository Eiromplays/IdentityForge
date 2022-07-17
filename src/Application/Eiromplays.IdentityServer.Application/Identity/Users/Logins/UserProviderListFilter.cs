namespace Eiromplays.IdentityServer.Application.Identity.Users.Logins;

public class UserProviderListFilter : PaginationFilter
{
    public string UserId { get; set; } = default!;
}
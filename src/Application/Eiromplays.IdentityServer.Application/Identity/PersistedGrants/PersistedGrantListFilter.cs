namespace Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

public class PersistedGrantListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}
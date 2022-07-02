using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

public sealed class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }

    public DateTime Created { get; init; }
    public string? CreatedBy { get; init; }

    public DateTime? LastModified { get; init; }
    public string? LastModifiedBy { get; init; }

    public ApplicationRole(string name, string? description = null)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }
}
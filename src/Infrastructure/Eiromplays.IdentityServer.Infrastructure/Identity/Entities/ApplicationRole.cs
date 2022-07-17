using Eiromplays.IdentityServer.Domain.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

public sealed class ApplicationRole : IdentityRole, IAuditableEntity
{
    public string? Description { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public ApplicationRole(string name, string? description = null)
        : base(name)
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }

    public ApplicationRole()
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }
}
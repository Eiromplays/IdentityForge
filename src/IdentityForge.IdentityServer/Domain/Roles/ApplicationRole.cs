using Microsoft.AspNetCore.Identity;

namespace IdentityForge.IdentityServer.Domain.Roles;

public sealed class ApplicationRole : IdentityRole, IBaseEntity
{
    public string? Description { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public required string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public required string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public List<DomainEvent> DomainEvents { get; } = new();

    public ApplicationRole(string name, string? description = null)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }
}
using Microsoft.AspNetCore.Identity;

namespace IdentityForge.IdentityServer.Domain.Roles;

public class ApplicationRoleClaim : IdentityRoleClaim<string>, IBaseEntity
{
    public DateTimeOffset CreatedOn { get; set; }
    public required string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public required string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public List<DomainEvent> DomainEvents { get; } = new();
}
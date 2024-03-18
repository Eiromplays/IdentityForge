using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityForge.IdentityServer.Domain;

public abstract class BaseEntity : IBaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedOn { get; set; }
    public required string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public required string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new();
}
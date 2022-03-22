namespace Eiromplays.IdentityServer.Domain.Common;

public partial interface IAuditableEntity
{
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
    
    public abstract List<DomainEvent> DomainEvents { get; set; }
}
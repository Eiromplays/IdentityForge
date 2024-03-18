using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityForge.IdentityServer.Domain;

public interface IBaseEntity
{
    public DateTimeOffset CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    [NotMapped]
    public List<DomainEvent> DomainEvents => new();

    public void UpdateCreationProperties(DateTimeOffset createdOn, string createdBy)
    {
        CreatedOn = createdOn;
        CreatedBy = createdBy;
    }

    public void UpdateModifiedProperties(DateTimeOffset? lastModifiedOn, string lastModifiedBy)
    {
        LastModifiedOn = lastModifiedOn;
        LastModifiedBy = lastModifiedBy;
    }

    public void UpdateIsDeleted(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }

    public void QueueDomainEvent(DomainEvent domainEvent)
    {
        if(!DomainEvents.Contains(domainEvent))
            DomainEvents.Add(domainEvent);
    }
}
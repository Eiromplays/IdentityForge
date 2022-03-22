namespace Eiromplays.IdentityServer.Domain.Common
{
    public abstract class AuditableEntity : IAuditableEntity
    {
        public DateTime Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }

        public List<DomainEvent>? DomainEvents { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using MassTransit;

namespace Eiromplays.IdentityServer.Domain.Common.Contracts;

public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity() => Id = NewId.Next().ToGuid();
}

public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; protected set; } = default!;

    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new();
}
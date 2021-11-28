using Eiromplays.IdentityServer.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eiromplays.IdentityServer.Domain.Events.Permission;

namespace Eiromplays.IdentityServer.Domain.Entities
{
    public class Permission : AuditableEntity, IHasDomainEvent
    {
        public Permission()
        {
        }

        public Permission(string? name) : this()
        {
            Name = name;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public override string? ToString()
            => Name;

        private bool _done;

        public bool Done
        {
            get => _done;
            set
            {
                if (value && !_done)
                {
                    DomainEvents.Add(new PermissionCompletedEvent(this));
                }

                _done = value;
            }
        }
    }
}
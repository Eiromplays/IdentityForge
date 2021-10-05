using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Domain.Entities;
using Eiromplays.IdentityServer.Domain.Events.Permission;
using MediatR;

namespace Eiromplays.IdentityServer.Application.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommand : IRequest<string>
    {
        public string? Name { get; set; }
    }

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, string>
    {
        private readonly IPermissionDbContext _context;

        public CreatePermissionCommandHandler(IPermissionDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = new Permission(request.Name){ Done = false };

            entity.DomainEvents.Add(new PermissionCreatedEvent(entity));

            if (_context.Permissions != null)
            {
                await _context.Permissions.AddAsync(entity, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id.ToString();
        }
    }
}

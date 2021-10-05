using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interface;
using Eiromplays.IdentityServer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Application.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommand : IRequest
    {
        public string Id { get; set; } = "";

        public string Name { get; set; } = "";

        public bool Done { get; set; }
    }

    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand>
    {
        private readonly IPermissionDbContext _context;

        public UpdatePermissionCommandHandler(IPermissionDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Permissions!.FirstOrDefaultAsync(x => x.Id.ToString().Equals(request.Id),
                cancellationToken: cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Permission), request.Id);
            }

            entity.Name = request.Name;
            entity.Done = request.Done;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

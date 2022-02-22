using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Eiromplays.IdentityServer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Application.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommand : IRequest
    {
        public string Id { get; set; } = "";
    }

    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand>
    {
        private readonly IPermissionDbContext _context;

        public DeletePermissionCommandHandler(IPermissionDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Permissions.FirstOrDefaultAsync(x => x.Id.ToString().Equals(request.Id),
                cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Permission), request.Id);
            }

            _context.Permissions.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

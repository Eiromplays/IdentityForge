using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<List<PersistedGrantDto>> GetPersistedGrantsAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(_t["User Not Found."]);

        return (await _db.PersistedGrants.Where(x => x.SubjectId == userId).ToListAsync(cancellationToken)).Adapt<List<PersistedGrantDto>>();
    }
}
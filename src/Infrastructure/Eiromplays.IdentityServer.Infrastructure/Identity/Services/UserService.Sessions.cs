using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<bool> RemoveSessionsAsync(string userId, CancellationToken cancellationToken)
    {
        _sessionDbContext.UserSessions
            .RemoveRange(await _sessionDbContext.UserSessions.Where(x => 
                x.SubjectId.Equals(userId)).ToListAsync(cancellationToken));

        return await _sessionDbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
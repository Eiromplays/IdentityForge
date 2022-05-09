using Ardalis.Specification.EntityFrameworkCore;
using Duende.Bff.EntityFramework;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    public async Task<PaginationResponse<UserSessionDto>> SearchSessionsAsync(UserSessionListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<UserSessionEntity>(filter);

        var sessions = await _sessionDbContext.UserSessions.WithSpecification(spec).ProjectToType<UserSessionDto>()
            .ToListAsync(cancellationToken);

        var count = await _sessionDbContext.UserSessions
            .CountAsync(cancellationToken);

        return new PaginationResponse<UserSessionDto>(sessions, count, filter.PageNumber, filter.PageSize);
    }
    
    public async Task<List<UserSessionDto>> GetAllUserSessions(CancellationToken cancellationToken)
    {
        return (await _sessionDbContext.UserSessions.AsNoTracking().ToListAsync(cancellationToken))
            .Adapt<List<UserSessionDto>>();
    }
    
    public async Task<bool> RemoveSessionsAsync(string userId, CancellationToken cancellationToken)
    {
        _sessionDbContext.UserSessions
            .RemoveRange(await _sessionDbContext.UserSessions.Where(x => 
                x.SubjectId.Equals(userId)).ToListAsync(cancellationToken));

        return await _sessionDbContext.SaveChangesAsync(cancellationToken) > 0;
    }
    
    public async Task<List<UserSessionDto>> GetUserSessionsAsync(string userId, CancellationToken cancellationToken)
    {
        var userSessions = (await _sessionDbContext.UserSessions
            .Where(x => x.SubjectId.Equals(userId))
            .ToListAsync(cancellationToken)).Adapt<List<UserSessionDto>>();

        return userSessions;
    }

    public async Task<UserSessionDto> GetUserSessionAsync(string key, string? userId, CancellationToken cancellationToken)
    {
        var userSession =
            (await _sessionDbContext.UserSessions.Where(x => x.Key == key).FirstOrDefaultAsync(cancellationToken))
            ?.Adapt<UserSessionDto>();

        _ = userSession ?? throw new NotFoundException(_t["User Session Not Found."]);
        
        if (!string.IsNullOrWhiteSpace(userId) && !userSession.SubjectId.Equals(userId))
            throw new UnauthorizedException(_t["No Access to User Session."]);
        
        return userSession;
    }
    
    public async Task<string> DeleteUserSessionAsync(string key, string? userId, CancellationToken cancellationToken)
    {
        var userSession = await _sessionDbContext.UserSessions.Where(x => x.Key == key).FirstOrDefaultAsync(cancellationToken);

        _ = userSession ?? throw new NotFoundException(_t["User Session Not Found."]);
        
        if (!string.IsNullOrWhiteSpace(userId) && !userSession.SubjectId.Equals(userId))
            throw new UnauthorizedException(_t["No Access to User Session."]);
        
        _sessionDbContext.UserSessions.Remove(userSession);

        await _sessionDbContext.SaveChangesAsync(cancellationToken);
        
        return string.Format(_t["User Session {0} Deleted."], userSession.Key);
    }
}
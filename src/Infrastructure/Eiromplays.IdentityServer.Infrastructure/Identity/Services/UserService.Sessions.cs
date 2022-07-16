using Ardalis.Specification.EntityFrameworkCore;
using Duende.Bff.EntityFramework;
using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class UserService
{
    #region Bff Sessions

    public async Task<PaginationResponse<UserSessionDto>> SearchBffSessionsAsync(UserSessionListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<UserSessionEntity>(filter);

        var sessions = await _sessionDbContext.UserSessions.WithSpecification(spec).ProjectToType<UserSessionDto>()
            .ToListAsync(cancellationToken);

        int count = await _sessionDbContext.UserSessions
            .CountAsync(cancellationToken);

        return new PaginationResponse<UserSessionDto>(sessions, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<List<UserSessionDto>> GetAllBffUserSessionsAsync(CancellationToken cancellationToken)
    {
        return (await _sessionDbContext.UserSessions.AsNoTracking().ToListAsync(cancellationToken))
            .Adapt<List<UserSessionDto>>();
    }

    public async Task<bool> RemoveBffSessionsAsync(string userId, CancellationToken cancellationToken)
    {
        _sessionDbContext.UserSessions
            .RemoveRange(await _sessionDbContext.UserSessions.Where(x =>
                x.SubjectId.Equals(userId)).ToListAsync(cancellationToken));

        return await _sessionDbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<List<UserSessionDto>> GetBffUserSessionsAsync(string userId, CancellationToken cancellationToken)
    {
        var userSessions = (await _sessionDbContext.UserSessions
            .Where(x => x.SubjectId.Equals(userId))
            .ToListAsync(cancellationToken)).Adapt<List<UserSessionDto>>();

        return userSessions;
    }

    public async Task<UserSessionDto> GetBffUserSessionAsync(string key, string? userId, CancellationToken cancellationToken)
    {
        var userSession =
            (await _sessionDbContext.UserSessions.Where(x => x.Key == key).FirstOrDefaultAsync(cancellationToken))
            ?.Adapt<UserSessionDto>();

        _ = userSession ?? throw new NotFoundException(_t["User Session Not Found."]);

        if (!string.IsNullOrWhiteSpace(userId) && !userSession.SubjectId.Equals(userId))
            throw new UnauthorizedException(_t["No Access to User Session."]);
        return userSession;
    }

    public async Task<string> DeleteBffUserSessionAsync(string key, string? userId, CancellationToken cancellationToken)
    {
        var userSession = await _sessionDbContext.UserSessions.Where(x => x.Key == key).FirstOrDefaultAsync(cancellationToken);

        _ = userSession ?? throw new NotFoundException(_t["User Session Not Found."]);

        if (!string.IsNullOrWhiteSpace(userId) && !userSession.SubjectId.Equals(userId))
            throw new UnauthorizedException(_t["No Access to User Session."]);

        _sessionDbContext.UserSessions.Remove(userSession);

        await _sessionDbContext.SaveChangesAsync(cancellationToken);

        return string.Format(_t["User Session {0} Deleted."], userSession.Key);
    }

    #endregion

    #region Server-Side Sessions

    public async Task<PaginationResponse<ServerSideSessionDto>> SearchServerSideSessionsAsync(ServerSideSessionListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<ServerSideSession>(filter);

        var sessions = await _db.ServerSideSessions.WithSpecification(spec).ProjectToType<ServerSideSessionDto>()
            .ToListAsync(cancellationToken);

        int count = await _db.ServerSideSessions
            .CountAsync(cancellationToken);

        return new PaginationResponse<ServerSideSessionDto>(sessions, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<List<ServerSideSessionDto>> GetAllServerSideSessionsAsync(CancellationToken cancellationToken)
    {
        return (await _db.ServerSideSessions.AsNoTracking().ToListAsync(cancellationToken))
            .Adapt<List<ServerSideSessionDto>>();
    }

    public async Task<bool> RemoveServerSideSessionsAsync(string userId, CancellationToken cancellationToken)
    {
        _db.ServerSideSessions
            .RemoveRange(await _db.ServerSideSessions.Where(x =>
                x.SubjectId.Equals(userId)).ToListAsync(cancellationToken));

        return await _db.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<List<ServerSideSessionDto>> GetServerSideSessionsAsync(string userId, CancellationToken cancellationToken)
    {
        var userSessions = (await _db.ServerSideSessions
            .Where(x => x.SubjectId.Equals(userId))
            .ToListAsync(cancellationToken)).Adapt<List<ServerSideSessionDto>>();

        return userSessions;
    }

    public async Task<ServerSideSessionDto> GetServerSideSessionAsync(string key, string? userId, CancellationToken cancellationToken)
    {
        var serverSideSession =
            (await _db.ServerSideSessions.Where(x => x.Key == key).FirstOrDefaultAsync(cancellationToken))
            ?.Adapt<ServerSideSessionDto>();

        _ = serverSideSession ?? throw new NotFoundException(_t["Server-side Session Not Found."]);

        if (!string.IsNullOrWhiteSpace(userId) && !serverSideSession.SubjectId.Equals(userId))
            throw new UnauthorizedException(_t["No Access to Server-side Session."]);
        return serverSideSession;
    }

    public async Task<string> DeleteServerSideSessionAsync(string key, string? userId, CancellationToken cancellationToken)
    {
        var serverSideSession = await _db.ServerSideSessions.Where(x => x.Key == key).FirstOrDefaultAsync(cancellationToken);

        _ = serverSideSession ?? throw new NotFoundException(_t["Server-side Session Not Found."]);

        if (!string.IsNullOrWhiteSpace(userId) && !serverSideSession.SubjectId.Equals(userId))
            throw new UnauthorizedException(_t["No Access to Server-side Session."]);

        _db.ServerSideSessions.Remove(serverSideSession);

        await _db.SaveChangesAsync(cancellationToken);

        return string.Format(_t["User Session {0} Deleted."], serverSideSession.Key);
    }

    #endregion
}
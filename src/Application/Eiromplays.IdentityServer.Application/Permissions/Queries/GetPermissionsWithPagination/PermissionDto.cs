using Eiromplays.IdentityServer.Application.Common.Mappings;
using Eiromplays.IdentityServer.Domain.Entities;

namespace Eiromplays.IdentityServer.Application.Permissions.Queries.GetPermissionsWithPagination;

public class PermissionDto<TKey> : IMapValues<Permission>
{
    public TKey? Id { get; set; }

    public string? Name { get; set; }

    public bool Done { get; set; }
}
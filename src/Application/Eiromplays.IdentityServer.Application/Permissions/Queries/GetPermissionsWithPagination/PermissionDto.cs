using AutoMapper;
using Eiromplays.IdentityServer.Domain.Entities;

namespace Eiromplays.IdentityServer.Application.Permissions.Queries.GetPermissionsWithPagination;

[AutoMap(typeof(Permission), ReverseMap = true)]
public class PermissionDto
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public bool Done { get; set; }
}
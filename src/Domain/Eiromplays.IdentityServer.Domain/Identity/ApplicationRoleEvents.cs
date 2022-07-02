﻿namespace Eiromplays.IdentityServer.Domain.Identity;

public abstract class ApplicationRoleEvent : DomainEvent
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    protected ApplicationRoleEvent(string roleId, string roleName) =>
        (RoleId, RoleName) = (roleId, roleName);
}

public class ApplicationRoleCreatedEvent : ApplicationRoleEvent
{
    public ApplicationRoleCreatedEvent(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}

public class ApplicationRoleUpdatedEvent : ApplicationRoleEvent
{
    public bool PermissionsUpdated { get; set; }

    public ApplicationRoleUpdatedEvent(string roleId, string roleName, bool permissionsUpdated = false)
        : base(roleId, roleName) =>
        PermissionsUpdated = permissionsUpdated;
}

public class ApplicationRoleDeletedEvent : ApplicationRoleEvent
{
    public bool PermissionsUpdated { get; set; }

    public ApplicationRoleDeletedEvent(string roleId, string roleName)
        : base(roleId, roleName)
    {
    }
}
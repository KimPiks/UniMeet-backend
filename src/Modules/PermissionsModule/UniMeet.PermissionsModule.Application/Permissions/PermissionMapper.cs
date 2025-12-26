using PermissionsModule.Domain.Permissions;

namespace PermissionsModule.Application.Permissions;

public static class PermissionMapper
{
    public static PermissionDto ToDto(this Permission permission)
    {
        return new PermissionDto
        {
            Id = permission.Id,
            PermissionName = permission.PermissionName,
            CreatedAtUtc = permission.CreatedAtUtc
        };
    }
}
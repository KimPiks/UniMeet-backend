using ModularSystem.Contracts.Permissions.Permissions;
using PermissionsModule.Application.Permissions;
using PermissionsModule.Domain.Groups;

namespace PermissionsModule.Application.Groups;

public static class GroupMapper
{
    public static GroupDto ToDto(this Group group)
    {
        return new GroupDto()
        {
            Id = group.Id,
            Name = group.Name,
            CreatedAtUtc = group.CreatedAtUtc,
            UpdatedAtUtc = group.UpdatedAtUtc,
            Permissions = group.Permissions.Select(p => p.ToDto()).ToList()
        };
    }
}

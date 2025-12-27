using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Permissions.GetPermissionsForGroup;

public record GetPermissionsForGroupQuery(int GroupId) : IQuery<IEnumerable<PermissionDto>>;
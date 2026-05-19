using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Permissions.Permissions.GetPermissionsForGroup;

public record GetPermissionsForGroupQuery(int GroupId) : IQuery<IEnumerable<PermissionDto>>;
using PermissionsModule.Domain.Permissions;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Permissions.GetPermissionsForGroup;

public class GetPermissionsForGroupQueryHandler(IPermissionRepository permissionRepository) : IQueryHandler<GetPermissionsForGroupQuery, IEnumerable<PermissionDto>>
{
    public async Task<IEnumerable<PermissionDto>> HandleAsync(GetPermissionsForGroupQuery request, CancellationToken cancellationToken = default)
    {
        var permissions = await permissionRepository.GetAllByGroupIdAsync(request.GroupId, cancellationToken);
        return permissions.Select(p => p.ToDto());
    }
}
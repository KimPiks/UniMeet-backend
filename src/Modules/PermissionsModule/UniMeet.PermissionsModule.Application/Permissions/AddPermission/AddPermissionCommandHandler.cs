using PermissionsModule.Domain.Permissions;
using PermissionsModule.Domain.Permissions.Exceptions;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Permissions.AddPermission;

public class AddPermissionCommandHandler(IPermissionRepository permissionRepository) : ICommandHandler<AddPermissionCommand>
{
    public async Task HandleAsync(AddPermissionCommand request, CancellationToken cancellationToken = default)
    {
        request.Validate();

        var permissions = await permissionRepository.GetAllByGroupIdAsync(request.GroupId, cancellationToken);
        var dbPermission = permissions.FirstOrDefault(p => p.PermissionName == request.PermissionName);
        if (dbPermission != null)
            throw new PermissionAlreadyExistsException(dbPermission.Group.Name, request.PermissionName);
        
        var permission = new Permission(request.GroupId, request.PermissionName);
        await permissionRepository.AddAsync(permission, cancellationToken);
        await permissionRepository.SaveChangesAsync(cancellationToken);
    }
}
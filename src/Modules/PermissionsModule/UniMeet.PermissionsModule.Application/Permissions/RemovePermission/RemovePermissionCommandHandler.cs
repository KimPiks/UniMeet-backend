using PermissionsModule.Domain.Permissions;
using PermissionsModule.Domain.Permissions.Exceptions;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Permissions.RemovePermission;

public class RemovePermissionCommandHandler(IPermissionRepository permissionRepository) : ICommandHandler<RemovePermissionCommand>
{
    public async Task HandleAsync(RemovePermissionCommand request, CancellationToken cancellationToken = default)
    {
        var permission = await permissionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (permission == null)
            throw new PermissionNotFoundException(request.Id);
        
        permissionRepository.Delete(permission);
        await permissionRepository.SaveChangesAsync(cancellationToken);
    }
}
using System.ComponentModel.DataAnnotations;
using PermissionsModule.Domain.Groups.Exceptions;
using PermissionsModule.Domain.Permissions;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.RemoveGroup;

public class RemoveGroupCommandHandler(IPermissionRepository permissionRepository) : ICommandHandler<RemoveGroupCommand>
{
    public async Task HandleAsync(RemoveGroupCommand request, CancellationToken cancellationToken = default)
    {
        var group = await permissionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (group == null)
            throw new GroupWithIdNotFoundException(request.Id);
        
        permissionRepository.Delete(group);
        await permissionRepository.SaveChangesAsync(cancellationToken);
    }
}
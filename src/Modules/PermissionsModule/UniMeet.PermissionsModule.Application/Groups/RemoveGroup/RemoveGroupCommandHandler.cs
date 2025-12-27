using System.ComponentModel.DataAnnotations;
using PermissionsModule.Domain.Groups;
using PermissionsModule.Domain.Groups.Exceptions;
using PermissionsModule.Domain.Permissions;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.RemoveGroup;

public class RemoveGroupCommandHandler(IGroupRepository groupRepository) : ICommandHandler<RemoveGroupCommand>
{
    public async Task HandleAsync(RemoveGroupCommand request, CancellationToken cancellationToken = default)
    {
        var group = await groupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (group == null)
            throw new GroupWithIdNotFoundException(request.Id);
        
        groupRepository.Delete(group);
        await groupRepository.SaveChangesAsync(cancellationToken);
    }
}
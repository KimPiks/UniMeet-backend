using PermissionsModule.Domain.Groups;
using PermissionsModule.Domain.Groups.Exceptions;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.UpdateGroup;

public class UpdateGroupCommandHandler(IGroupRepository groupRepository) : ICommandHandler<UpdateGroupCommand>
{
    public async Task HandleAsync(UpdateGroupCommand request, CancellationToken cancellationToken = default)
    {
        request.Validate();

        var group = await groupRepository.GetByIdAsync(request.Id, cancellationToken);
        if (group == null)
            throw new GroupWithIdNotFoundException(request.Id);

        group.Rename(request.NewName);
        await groupRepository.SaveChangesAsync(cancellationToken);
    }
}
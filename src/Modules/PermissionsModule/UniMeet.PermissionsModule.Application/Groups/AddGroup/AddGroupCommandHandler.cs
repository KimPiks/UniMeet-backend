using PermissionsModule.Domain.Groups;
using PermissionsModule.Domain.Groups.Exceptions;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.AddGroup;

public class AddGroupCommandHandler(IGroupRepository groupRepository) : ICommandHandler<AddGroupCommand>
{
    public async Task HandleAsync(AddGroupCommand request, CancellationToken cancellationToken = default)
    {
        request.Validate();

        var groupAlreadyExists = await groupRepository.GetByNameAsync(request.Name, cancellationToken) != null;
        if (groupAlreadyExists)
            throw new GroupAlreadyExistsException(request.Name);
        
        var group = new Group(request.Name);
        await groupRepository.AddAsync(group, cancellationToken);
        await groupRepository.SaveChangesAsync(cancellationToken);
    }
}
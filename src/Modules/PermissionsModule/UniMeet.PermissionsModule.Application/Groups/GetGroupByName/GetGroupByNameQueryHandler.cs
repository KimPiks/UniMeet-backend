using PermissionsModule.Domain.Groups;
using PermissionsModule.Domain.Groups.Exceptions;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.GetGroupByName;

public class GetGroupByNameQueryHandler(IGroupRepository groupRepository) : IQueryHandler<GetGroupByNameQuery, GroupDto>
{
    public async Task<GroupDto> HandleAsync(GetGroupByNameQuery request, CancellationToken cancellationToken = default)
    {
        var group = await groupRepository.GetByNameAsync(request.Name, cancellationToken);
        if (group == null)
            throw new GroupNotFoundException(request.Name);
        
        return group.ToDto();
    }
}
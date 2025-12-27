using PermissionsModule.Domain.Groups;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.GetAllGroups;

public class GetAllGroupsQueryHandler(IGroupRepository groupRepository) : IQueryHandler<GetAllGroupsQuery, IEnumerable<GroupDto>>
{
    public async Task<IEnumerable<GroupDto>> HandleAsync(GetAllGroupsQuery request, CancellationToken cancellationToken = default)
    {
        var groups = await groupRepository.GetAllAsync(request.Offset, request.Limit, cancellationToken);
        return groups.Select(g => g.ToDto());
    }
}
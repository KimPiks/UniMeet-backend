using ModularSystem.Contracts.Permissions;
using PermissionsModule.Domain.Groups;
using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.GetGroupByName;

public class GetGroupLookupByNameQueryHandler(IGroupRepository groupRepository)
    : IQueryHandler<ModularSystem.Contracts.Permissions.GetGroupByNameQuery, GroupLookupDto?>
{
    public async Task<GroupLookupDto?> HandleAsync(
        ModularSystem.Contracts.Permissions.GetGroupByNameQuery request,
        CancellationToken cancellationToken = default)
    {
        var group = await groupRepository.GetByNameAsync(request.Name, cancellationToken);
        return group is null ? null : new GroupLookupDto(group.Id, group.Name);
    }
}

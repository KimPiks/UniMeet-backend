using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.GetAllGroups;

public record GetAllGroupsQuery(int Offset, int Limit) : IQuery<IEnumerable<GroupDto>>;
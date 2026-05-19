using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Permissions.Groups.GetAllGroups;

public record GetAllGroupsQuery(int Offset, int Limit) : IQuery<IEnumerable<GroupDto>>;
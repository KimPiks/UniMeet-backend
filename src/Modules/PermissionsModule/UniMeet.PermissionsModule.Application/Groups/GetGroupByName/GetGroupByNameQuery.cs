using UniMeet.Shared.Abstractions;

namespace PermissionsModule.Application.Groups.GetGroupByName;

public record GetGroupByNameQuery(string Name) : IQuery<GroupDto>;
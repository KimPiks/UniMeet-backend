using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Permissions.Groups.GetGroupByName;

public record GetGroupByNameQuery(string Name) : IQuery<GroupDto>;
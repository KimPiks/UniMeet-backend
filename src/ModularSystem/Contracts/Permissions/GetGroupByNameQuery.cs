using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Permissions;

public record GetGroupByNameQuery(string Name) : IQuery<GroupLookupDto?>;

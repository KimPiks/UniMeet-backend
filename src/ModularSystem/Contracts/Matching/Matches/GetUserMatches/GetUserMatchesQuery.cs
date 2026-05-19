using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Matching.Matches.GetUserMatches;

public record GetUserMatchesQuery(Guid UserId) : IQuery<IEnumerable<MatchDto>>;

using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Matches.GetUserMatches;

public record GetUserMatchesQuery(Guid UserId) : IQuery<IEnumerable<MatchDto>>;

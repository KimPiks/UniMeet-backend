using UniMeet.MatchingModule.Domain.Matches;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Matches.AreUsersMatched;

public class AreUsersMatchedQueryHandler(IMatchRepository matchRepository)
    : IQueryHandler<AreUsersMatchedQuery, bool>
{
    public Task<bool> HandleAsync(AreUsersMatchedQuery request, CancellationToken cancellationToken = default)
    {
        return matchRepository.ExistsAsync(request.UserAId, request.UserBId, cancellationToken);
    }
}

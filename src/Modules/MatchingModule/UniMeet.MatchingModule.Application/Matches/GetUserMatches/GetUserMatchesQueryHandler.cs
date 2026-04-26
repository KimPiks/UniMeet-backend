using UniMeet.MatchingModule.Domain.Matches;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Matches.GetUserMatches;

public class GetUserMatchesQueryHandler(IMatchRepository matchRepository)
    : IQueryHandler<GetUserMatchesQuery, IEnumerable<MatchDto>>
{
    public async Task<IEnumerable<MatchDto>> HandleAsync(GetUserMatchesQuery request, CancellationToken cancellationToken = default)
    {
        var matches = await matchRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        return matches.Select(m => new MatchDto(m.Id, m.User1Id, m.User2Id, m.CreatedAt));
    }
}

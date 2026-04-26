namespace UniMeet.MatchingModule.Domain.Matches;

public interface IMatchRepository
{
    Task<Match?> GetByUsersAsync(Guid userA, Guid userB, CancellationToken cancellationToken = default);
    Task<IEnumerable<Match>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid userA, Guid userB, CancellationToken cancellationToken = default);
    Task AddAsync(Match match, CancellationToken cancellationToken = default);
    void Delete(Match match);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

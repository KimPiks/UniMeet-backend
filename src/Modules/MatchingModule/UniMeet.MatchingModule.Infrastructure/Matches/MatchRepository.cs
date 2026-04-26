using Microsoft.EntityFrameworkCore;
using UniMeet.MatchingModule.Domain.Matches;

namespace UniMeet.MatchingModule.Infrastructure.Matches;

public class MatchRepository(MatchingContext context) : IMatchRepository
{
    public async Task<Match?> GetByUsersAsync(Guid userA, Guid userB, CancellationToken cancellationToken = default)
    {
        var (user1, user2) = userA.CompareTo(userB) < 0 ? (userA, userB) : (userB, userA);
        return await context.Matches
            .FirstOrDefaultAsync(m => m.User1Id == user1 && m.User2Id == user2, cancellationToken);
    }

    public async Task<IEnumerable<Match>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Matches
            .Where(m => m.User1Id == userId || m.User2Id == userId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid userA, Guid userB, CancellationToken cancellationToken = default)
    {
        var (user1, user2) = userA.CompareTo(userB) < 0 ? (userA, userB) : (userB, userA);
        return await context.Matches
            .AnyAsync(m => m.User1Id == user1 && m.User2Id == user2, cancellationToken);
    }

    public async Task AddAsync(Match match, CancellationToken cancellationToken = default)
    {
        await context.Matches.AddAsync(match, cancellationToken);
    }

    public void Delete(Match match)
    {
        context.Matches.Remove(match);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

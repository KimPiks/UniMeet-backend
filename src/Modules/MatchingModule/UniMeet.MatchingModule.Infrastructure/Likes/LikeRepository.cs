using Microsoft.EntityFrameworkCore;
using UniMeet.MatchingModule.Domain.Likes;

namespace UniMeet.MatchingModule.Infrastructure.Likes;

public class LikeRepository(MatchingContext context) : ILikeRepository
{
    public async Task<Like?> GetAsync(Guid likerId, Guid likedId, CancellationToken cancellationToken = default)
    {
        return await context.Likes
            .FirstOrDefaultAsync(l => l.LikerId == likerId && l.LikedId == likedId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid likerId, Guid likedId, CancellationToken cancellationToken = default)
    {
        return await context.Likes
            .AnyAsync(l => l.LikerId == likerId && l.LikedId == likedId, cancellationToken);
    }

    public async Task AddAsync(Like like, CancellationToken cancellationToken = default)
    {
        await context.Likes.AddAsync(like, cancellationToken);
    }

    public void Delete(Like like)
    {
        context.Likes.Remove(like);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

namespace UniMeet.MatchingModule.Domain.Likes;

public interface ILikeRepository
{
    Task<Like?> GetAsync(Guid likerId, Guid likedId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Like>> GetByLikerIdAsync(Guid likerId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid likerId, Guid likedId, CancellationToken cancellationToken = default);
    Task AddAsync(Like like, CancellationToken cancellationToken = default);
    void Delete(Like like);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

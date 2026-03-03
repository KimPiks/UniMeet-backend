namespace UniMeet.UserModule.Domain.Interests;

public interface IInterestRepository
{
    Task<Interest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Interest>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task AddAsync(Interest interest, CancellationToken cancellationToken = default);
    void Delete(Interest interest);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


using UniMeet.UserModule.Domain.UserDetails;

namespace UniMeet.UserModule.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<User>> SearchAsync(
        int? universityId,
        Sex? sex,
        IReadOnlyCollection<int>? interestIds,
        IReadOnlyCollection<Guid>? userIds,
        CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    void Delete(User user);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
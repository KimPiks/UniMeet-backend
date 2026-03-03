namespace UniMeet.UserModule.Domain.UserDetails;

public interface IUserDetailRepository
{
    Task<UserDetail?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserDetail?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDetail>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task AddAsync(UserDetail userDetail, CancellationToken cancellationToken = default);
    void Delete(UserDetail userDetail);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}


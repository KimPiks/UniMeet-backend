namespace UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

public interface IUserAffiliationRepository
{
    Task<UserAffiliation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserAffiliation?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(UserAffiliation userAffiliation, CancellationToken cancellationToken = default);
    void Delete(UserAffiliation userAffiliation, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
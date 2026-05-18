namespace UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

public interface IUserAffiliationRepository
{
    Task<UserAffiliation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserAffiliation?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Guid>> GetUserIdsByFieldOfStudyIdAsync(int fieldOfStudyId, CancellationToken cancellationToken = default);
    Task<IDictionary<Guid, int>> GetFieldOfStudyIdsByUserIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
    Task AddAsync(UserAffiliation userAffiliation, CancellationToken cancellationToken = default);
    void Delete(UserAffiliation userAffiliation, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
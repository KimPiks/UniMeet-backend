using Microsoft.EntityFrameworkCore;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UserEnrollmentModule.Infrastructure.UserAffiliation;

public class UserAffiliationRepository(UserEnrollmentContext context) : IUserAffiliationRepository
{
    public async Task<Domain.UserAffiliation.UserAffiliation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.UserAffiliations
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Domain.UserAffiliation.UserAffiliation?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.UserAffiliations
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(Domain.UserAffiliation.UserAffiliation userAffiliation, CancellationToken cancellationToken = default)
    {
        await context.UserAffiliations.AddAsync(userAffiliation, cancellationToken);
    }

    public void Delete(Domain.UserAffiliation.UserAffiliation userAffiliation, CancellationToken cancellationToken = default)
    {
        context.UserAffiliations.Remove(userAffiliation);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Guid>> GetUserIdsByFieldOfStudyIdAsync(int fieldOfStudyId, CancellationToken cancellationToken = default)
    {
        return await context.UserAffiliations
            .Where(x => x.FieldOfStudyId == fieldOfStudyId)
            .Select(x => x.UserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IDictionary<Guid, int>> GetFieldOfStudyIdsByUserIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var userIdList = userIds.Distinct().ToList();
        if (userIdList.Count == 0)
        {
            return new Dictionary<Guid, int>();
        }

        return await context.UserAffiliations
            .Where(x => userIdList.Contains(x.UserId))
            .ToDictionaryAsync(x => x.UserId, x => x.FieldOfStudyId, cancellationToken);
    }
}
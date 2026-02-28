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
}
using Microsoft.EntityFrameworkCore;
using UniMeet.UserModule.Domain.Interests;

namespace UniMeet.UserModule.Infrastructure.Interests;

public class InterestRepository(UserContext context) : IInterestRepository
{
    private const int DefaultPageSize = 100;
    private const int MaxPageSize = 100;

    public async Task<Interest?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Interests
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Interest>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        var safeOffset = Math.Max(0, offset);
        var safeLimit = limit <= 0 ? DefaultPageSize : Math.Min(limit, MaxPageSize);
        return await context.Interests
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Skip(safeOffset)
            .Take(safeLimit)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Interest interest, CancellationToken cancellationToken = default)
    {
        await context.Interests.AddAsync(interest, cancellationToken);
    }

    public void Delete(Interest interest)
    {
        var trackedInterest = context.Interests.Local.FirstOrDefault(x => x.Id == interest.Id);
        context.Interests.Remove(trackedInterest ?? interest);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


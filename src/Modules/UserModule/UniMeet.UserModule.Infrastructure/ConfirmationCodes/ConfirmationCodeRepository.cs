using Microsoft.EntityFrameworkCore;
using UniMeet.UserModule.Domain.ConfirmationCodes;

namespace UniMeet.UserModule.Infrastructure.ConfirmationCodes;

public class ConfirmationCodeRepository(UserContext context) : IConfirmationCodeRepository
{
    public async Task<ConfirmationCode?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.ConfirmationCodes
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            
    }

    public async Task<ConfirmationCode?> GetByCodeAsync(Guid code, CancellationToken cancellationToken = default)
    {
        return await context.ConfirmationCodes
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<ConfirmationCode>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.ConfirmationCodes
            .Include(x => x.User)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ConfirmationCode user, CancellationToken cancellationToken = default)
    {
        await context.ConfirmationCodes.AddAsync(user, cancellationToken);
    }

    public void Delete(ConfirmationCode user)
    {
        context.ConfirmationCodes.Remove(user);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
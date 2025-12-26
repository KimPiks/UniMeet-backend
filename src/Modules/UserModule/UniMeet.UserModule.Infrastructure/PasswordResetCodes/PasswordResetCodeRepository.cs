using Microsoft.EntityFrameworkCore;
using UniMeet.UserModule.Domain.PasswordResetCodes;

namespace UniMeet.UserModule.Infrastructure.PasswordResetCodes;

public class PasswordResetCodeRepository(UserContext context) : IPasswordResetCodeRepository
{
    public async Task<PasswordResetCode?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.PasswordResetCodes
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PasswordResetCode?> GetByCodeAsync(Guid code, CancellationToken cancellationToken = default)
    {
        return await context.PasswordResetCodes
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<PasswordResetCode>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.PasswordResetCodes
            .Include(x => x.User)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PasswordResetCode code, CancellationToken cancellationToken = default)
    {
        await context.PasswordResetCodes.AddAsync(code, cancellationToken);
    }

    public void Delete(PasswordResetCode code)
    {
        context.PasswordResetCodes.Remove(code);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
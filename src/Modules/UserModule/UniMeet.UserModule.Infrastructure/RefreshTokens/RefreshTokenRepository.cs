using Microsoft.EntityFrameworkCore;
using UniMeet.UserModule.Domain.RefreshTokens;

namespace UniMeet.UserModule.Infrastructure.RefreshTokens;

public class RefreshTokenRepository(UserContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public async Task<IEnumerable<RefreshToken>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.RefreshTokens
            .Include(x => x.User)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        await context.RefreshTokens.AddAsync(token, cancellationToken);
    }

    public void Delete(RefreshToken token)
    {
        context.RefreshTokens.Remove(token);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
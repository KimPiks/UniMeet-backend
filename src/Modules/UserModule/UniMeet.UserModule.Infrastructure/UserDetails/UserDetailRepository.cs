using Microsoft.EntityFrameworkCore;
using UniMeet.UserModule.Domain.UserDetails;

namespace UniMeet.UserModule.Infrastructure.UserDetails;

public class UserDetailRepository(UserContext context) : IUserDetailRepository
{
    public async Task<UserDetail?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.UserDetails
            .Include(x => x.User)
            .Include(x => x.Interests)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<UserDetail?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.UserDetails
            .Include(x => x.User)
            .Include(x => x.Interests)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task<IEnumerable<UserDetail>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.UserDetails
            .Include(x => x.User)
            .Include(x => x.Interests)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(UserDetail userDetail, CancellationToken cancellationToken = default)
    {
        await context.UserDetails.AddAsync(userDetail, cancellationToken);
    }

    public void Delete(UserDetail userDetail)
    {
        context.UserDetails.Remove(userDetail);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}


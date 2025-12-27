using Microsoft.EntityFrameworkCore;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Infrastructure.Users;

public class UserRepository(UserContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .FirstOrDefaultAsync(x => x.Email == email.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public void Delete(User user)
    {
        context.Users.Remove(user);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
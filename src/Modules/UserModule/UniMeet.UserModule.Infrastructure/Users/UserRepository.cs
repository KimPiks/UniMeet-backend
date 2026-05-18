using Microsoft.EntityFrameworkCore;
using UniMeet.UserModule.Domain.UserDetails;
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

    public async Task<IReadOnlyList<User>> SearchAsync(
        int? universityId,
        Sex? sex,
        IReadOnlyCollection<int>? interestIds,
        IReadOnlyCollection<Guid>? userIds,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = context.Users
            .Include(u => u.UserDetail)
            .ThenInclude(d => d.Interests)
            .Where(u => u.IsActive);

        if (universityId.HasValue)
        {
            query = query.Where(u => u.UniversityId == universityId.Value);
        }

        if (sex.HasValue)
        {
            query = query.Where(u => u.UserDetail.Sex == sex.Value);
        }

        if (interestIds is { Count: > 0 })
        {
            query = query.Where(u => u.UserDetail.Interests.Any(i => interestIds.Contains(i.Id)));
        }

        if (userIds is { Count: > 0 })
        {
            query = query.Where(u => userIds.Contains(u.Id));
        }

        if (userIds is { Count: 0 })
        {
            return Array.Empty<User>();
        }

        return await query.ToListAsync(cancellationToken);
    }
}
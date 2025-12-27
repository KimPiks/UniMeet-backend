using Microsoft.EntityFrameworkCore;
using PermissionsModule.Domain.Groups;

namespace PermissionsModule.Infrastructure.Groups;

public class GroupRepository(PermissionsContext context) : IGroupRepository
{
    public async Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Groups
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Group?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await context.Groups
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Group>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.Groups
            .Include(x => x.Permissions)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Group group, CancellationToken cancellationToken = default)
    {
        await context.Groups.AddAsync(group, cancellationToken);
    }

    public void UpdateAsync(Group group, CancellationToken cancellationToken = default)
    {
        context.Groups.Update(group);
    }

    public void Delete(Group group)
    {
        context.Groups.Remove(group);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
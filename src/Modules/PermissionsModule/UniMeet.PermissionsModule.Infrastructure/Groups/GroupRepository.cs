using Microsoft.EntityFrameworkCore;
using PermissionsModule.Domain.Groups;

namespace PermissionsModule.Infrastructure.Groups;

public class GroupRepository(PermissionsContext context) : IGroupRepository
{
    private const int DefaultPageSize = 100;
    private const int MaxPageSize = 100;

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
        var safeOffset = Math.Max(0, offset);
        var safeLimit = limit <= 0 ? DefaultPageSize : Math.Min(limit, MaxPageSize);
        return await context.Groups
            .AsNoTracking()
            .Include(x => x.Permissions)
            .OrderBy(x => x.Id)
            .Skip(safeOffset)
            .Take(safeLimit)
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
        var trackedGroup = context.Groups.Local.FirstOrDefault(x => x.Id == group.Id);
        context.Groups.Remove(trackedGroup ?? group);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

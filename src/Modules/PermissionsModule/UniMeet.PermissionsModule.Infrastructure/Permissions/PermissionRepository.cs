using Microsoft.EntityFrameworkCore;
using PermissionsModule.Domain.Permissions;

namespace PermissionsModule.Infrastructure.Permissions;

public class PermissionRepository(PermissionsContext context) : IPermissionRepository
{
    private const int DefaultPageSize = 100;
    private const int MaxPageSize = 100;

    public async Task<Permission?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Permissions
            .Include(x => x.Group)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        var safeOffset = Math.Max(0, offset);
        var safeLimit = limit <= 0 ? DefaultPageSize : Math.Min(limit, MaxPageSize);
        return await context.Permissions
            .AsNoTracking()
            .Include(x => x.Group)
            .OrderBy(x => x.PermissionName)
            .ThenBy(x => x.Id)
            .Skip(safeOffset)
            .Take(safeLimit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetAllByGroupIdAsync(int groupId,
        CancellationToken cancellationToken = default)
    {
        return await context.Permissions
            .Include(x => x.Group)
            .Where(x => x.GroupId == groupId)
            .ToListAsync(cancellationToken);
    }


    public async Task AddAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        await context.Permissions.AddAsync(permission, cancellationToken);
    }

    public void Delete(Permission permission)
    {
        var trackedPermission = context.Permissions.Local.FirstOrDefault(x => x.Id == permission.Id);
        context.Permissions.Remove(trackedPermission ?? permission);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

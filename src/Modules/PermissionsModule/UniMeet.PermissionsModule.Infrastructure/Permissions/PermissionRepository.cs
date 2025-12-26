using Microsoft.EntityFrameworkCore;
using PermissionsModule.Domain.Permissions;

namespace PermissionsModule.Infrastructure.Permissions;

public class PermissionRepository(PermissionsContext context) : IPermissionRepository
{
    public async Task<Permission?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Permissions
            .Include(x => x.Group)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.Permissions
            .Include(x => x.Group)
            .Skip(offset)
            .Take(limit)
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
        context.Permissions.Remove(permission);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
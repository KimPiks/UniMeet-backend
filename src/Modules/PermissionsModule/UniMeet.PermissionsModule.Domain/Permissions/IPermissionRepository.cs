namespace PermissionsModule.Domain.Permissions;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetAllByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);
    Task AddAsync(Permission permission, CancellationToken cancellationToken = default);
    void Delete(Permission permission);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
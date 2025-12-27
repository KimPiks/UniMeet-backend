namespace PermissionsModule.Domain.Groups;

public interface IGroupRepository
{
    Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Group?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Group>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default);
    Task AddAsync(Group group, CancellationToken cancellationToken = default);
    void UpdateAsync(Group group, CancellationToken cancellationToken = default);
    void Delete(Group group);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
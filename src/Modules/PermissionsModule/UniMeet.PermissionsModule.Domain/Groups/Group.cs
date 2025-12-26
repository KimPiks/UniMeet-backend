using PermissionsModule.Domain.Permissions;

namespace PermissionsModule.Domain.Groups;

public class Group
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }
    public List<Permission> Permissions { get; private set; }
    
    private Group() { }

    public Group(string name)
    {
        Name = name;
        Permissions = new List<Permission>();
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Rename(string newName)
    {
        Name = newName;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void AddPermission(Permission permission)
    {
        Permissions.Add(permission);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void RemovePermission(Permission permission)
    {
        Permissions.Remove(permission);
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
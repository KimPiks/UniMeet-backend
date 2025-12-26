using PermissionsModule.Domain.Groups;

namespace PermissionsModule.Domain.Permissions;

public class Permission
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public string PermissionName { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    
    private Permission() { }
    
    public Permission(int groupId, string permissionName)
    {
        GroupId = groupId;
        PermissionName = permissionName;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
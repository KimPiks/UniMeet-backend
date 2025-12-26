using PermissionsModule.Application.Permissions;

namespace PermissionsModule.Application.Groups;

public class GroupDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
    public required DateTime UpdatedAtUtc { get; set; }
    public required ICollection<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
}
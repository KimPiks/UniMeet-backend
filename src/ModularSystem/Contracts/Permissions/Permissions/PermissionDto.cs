namespace ModularSystem.Contracts.Permissions.Permissions;

public class PermissionDto
{
    public required int Id { get; set; }
    public required string PermissionName { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
}
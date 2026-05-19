using PermissionsModule.Domain.Groups;
using PermissionsModule.Domain.Permissions;

namespace UniMeet.UnitTests.Modules;

public class PermissionsDomainTests
{
    [Fact]
    public void Group_AddPermission_adds_permission_and_updates_timestamp()
    {
        var group = new Group("User");
        var previousUpdatedAt = group.UpdatedAtUtc;
        var permission = new Permission(group.Id, "UserModule.GetUsers");

        group.AddPermission(permission);

        Assert.Same(permission, group.Permissions.Single());
        Assert.True(group.UpdatedAtUtc >= previousUpdatedAt);
    }

    [Fact]
    public void Group_Rename_changes_name_and_updates_timestamp()
    {
        var group = new Group("User");
        var previousUpdatedAt = group.UpdatedAtUtc;

        group.Rename("Admin");

        Assert.Equal("Admin", group.Name);
        Assert.True(group.UpdatedAtUtc >= previousUpdatedAt);
    }
}

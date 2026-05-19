using PermissionsModule.Domain.Groups;
using PermissionsModule.Domain.Permissions;
using PermissionsModule.Infrastructure.Groups;
using PermissionsModule.Infrastructure.Permissions;

namespace UniMeet.UnitTests.Infrastructure;

public class PermissionsRepositoryTests
{
    [Fact]
    public async Task GroupRepository_adds_loads_includes_updates_and_deletes_group()
    {
        await using var context = RepositoryTestContextFactory.CreatePermissionsContext();
        var repository = new GroupRepository(context);
        var group = new Group("Students");
        group.AddPermission(new Permission(group.Id, "UserModule.GetUsers"));

        await repository.AddAsync(group);
        await repository.SaveChangesAsync();
        var byName = await repository.GetByNameAsync("Students");
        byName!.Rename("Members");
        repository.UpdateAsync(byName);
        await repository.SaveChangesAsync();
        var paged = (await repository.GetAllAsync(0, 10)).Single();
        repository.Delete(paged);
        await repository.SaveChangesAsync();

        Assert.NotNull(byName);
        Assert.Single(byName.Permissions);
        Assert.Equal("Members", paged.Name);
        Assert.Empty(await repository.GetAllAsync(0, 10));
    }

    [Fact]
    public async Task PermissionRepository_adds_loads_by_group_and_deletes_permission()
    {
        await using var context = RepositoryTestContextFactory.CreatePermissionsContext();
        var group = new Group("Students");
        context.Groups.Add(group);
        await context.SaveChangesAsync();
        var repository = new PermissionRepository(context);
        var permission = new Permission(group.Id, "MessagingModule.SendMessage");

        await repository.AddAsync(permission);
        await repository.SaveChangesAsync();
        var byId = await repository.GetByIdAsync(permission.Id);
        var byGroup = (await repository.GetAllByGroupIdAsync(group.Id)).Single();
        var all = (await repository.GetAllAsync(0, 10)).Single();
        repository.Delete(permission);
        await repository.SaveChangesAsync();

        Assert.NotNull(byId);
        Assert.Equal(group.Id, byGroup.GroupId);
        Assert.Equal("Students", all.Group.Name);
        Assert.Empty(await repository.GetAllByGroupIdAsync(group.Id));
    }
}

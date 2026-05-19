using ModularSystem.Contracts.Permissions.Groups;
using ModularSystem.Contracts.Permissions.Groups.AddGroup;
using ModularSystem.Contracts.Permissions.Groups.GetAllGroups;
using ModularSystem.Contracts.Permissions.Groups.GetGroupByName;
using ModularSystem.Contracts.Permissions.Groups.RemoveGroup;
using ModularSystem.Contracts.Permissions.Groups.UpdateGroup;
using ModularSystem.Contracts.Permissions.Permissions;
using ModularSystem.Contracts.Permissions.Permissions.AddPermission;
using ModularSystem.Contracts.Permissions.Permissions.GetPermissionsForGroup;
using ModularSystem.Contracts.Permissions.Permissions.RemovePermission;
using UniMeet.API.Controllers.Group;
using UniMeet.API.Controllers.Permission;
using UniMeet.API.Models.Requests;

namespace UniMeet.UnitTests.Api;

public class PermissionsControllersTests
{
    [Fact]
    public async Task GroupController_endpoints_forward_requests_and_return_expected_responses()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var group = CreateGroupDto();
        dispatcher.QueueResult<IEnumerable<GroupDto>>([group]);
        dispatcher.QueueResult(group);
        var controller = new GroupController(dispatcher);

        var groups = ControllerTestHelpers.AssertOkResponse<IEnumerable<GroupDto>>(
            await controller.GetGroups(2, 25),
            "Groups retrieved successfully");
        var found = ControllerTestHelpers.AssertOkResponse<GroupDto>(
            await controller.GetGroup("Students"),
            "Group retrieved successfully");
        var added = ControllerTestHelpers.AssertOkResponse<string>(
            await controller.AddGroup(new AddGroupRequest { Name = "Students" }),
            "Group added successfully");
        var deleted = ControllerTestHelpers.AssertOkResponse<string>(
            await controller.DeleteGroup(new DeleteGroupRequest { GroupId = 7 }),
            "Group deleted successfully");
        var updated = ControllerTestHelpers.AssertOkResponse<string>(
            await controller.UpdateGroup(new UpdateGroupRequest { GroupId = 7, Name = "Admins" }),
            "Group updated successfully");

        Assert.Same(group, Assert.Single(groups.Data));
        Assert.Same(group, found.Data);
        Assert.Null(added.Data);
        Assert.Null(deleted.Data);
        Assert.Null(updated.Data);
        Assert.Collection(
            dispatcher.SentRequests,
            request => Assert.Equal(new GetAllGroupsQuery(2, 25), Assert.IsType<GetAllGroupsQuery>(request)),
            request => Assert.Equal(new GetGroupByNameQuery("Students"), Assert.IsType<GetGroupByNameQuery>(request)),
            request => Assert.Equal(new AddGroupCommand("Students"), Assert.IsType<AddGroupCommand>(request)),
            request => Assert.Equal(new RemoveGroupCommand(7), Assert.IsType<RemoveGroupCommand>(request)),
            request => Assert.Equal(new UpdateGroupCommand(7, "Admins"), Assert.IsType<UpdateGroupCommand>(request)));
    }

    [Fact]
    public async Task PermissionController_endpoints_forward_requests_and_return_expected_responses()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        var permission = new PermissionDto
        {
            Id = 11,
            PermissionName = "Users.Read",
            CreatedAtUtc = DateTime.UtcNow
        };
        dispatcher.QueueResult<IEnumerable<PermissionDto>>([permission]);
        var controller = new PermissionController(dispatcher);

        var added = ControllerTestHelpers.AssertOkResponse<string>(
            await controller.AddPermission(new AddPermissionRequest { GroupId = 7, PermissionName = "Users.Read" }),
            "Permission added successfully");
        var permissions = ControllerTestHelpers.AssertOkResponse<IEnumerable<PermissionDto>>(
            await controller.GetPermission(7),
            "Permissions retrieved successfully");
        var deleted = ControllerTestHelpers.AssertOkResponse<string>(
            await controller.DeletePermission(new DeletePermissionRequest { PermissionId = 11 }),
            "Permission deleted successfully");

        Assert.Null(added.Data);
        Assert.Same(permission, Assert.Single(permissions.Data));
        Assert.Null(deleted.Data);
        Assert.Collection(
            dispatcher.SentRequests,
            request => Assert.Equal(new AddPermissionCommand(7, "Users.Read"), Assert.IsType<AddPermissionCommand>(request)),
            request => Assert.Equal(new GetPermissionsForGroupQuery(7), Assert.IsType<GetPermissionsForGroupQuery>(request)),
            request => Assert.Equal(new RemovePermissionCommand(11), Assert.IsType<RemovePermissionCommand>(request)));
    }

    private static GroupDto CreateGroupDto()
        => new()
        {
            Id = 7,
            Name = "Students",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            Permissions = []
        };
}

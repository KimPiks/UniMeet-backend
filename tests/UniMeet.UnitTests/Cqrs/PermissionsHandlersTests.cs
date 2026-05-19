using ModularSystem.Contracts.Permissions.Groups.AddGroup;
using ModularSystem.Contracts.Permissions.Groups.GetAllGroups;
using PermissionsModule.Application.Groups.AddGroup;
using PermissionsModule.Application.Groups.GetAllGroups;
using PermissionsModule.Domain.Groups;
using PermissionsModule.Domain.Groups.Exceptions;
using UniMeet.Shared.Exceptions;

namespace UniMeet.UnitTests.Cqrs;

public class PermissionsHandlersTests
{
    [Fact]
    public async Task AddGroupCommandHandler_adds_new_group_and_saves_changes()
    {
        var repository = new FakeGroupRepository();
        var handler = new AddGroupCommandHandler(repository);

        await handler.HandleAsync(new AddGroupCommand("Students"));

        var group = Assert.Single(repository.Groups);
        Assert.Equal("Students", group.Name);
        Assert.Equal(1, repository.SaveChangesCalls);
    }

    [Fact]
    public async Task AddGroupCommandHandler_rejects_existing_group()
    {
        var repository = new FakeGroupRepository();
        repository.Groups.Add(new Group("Students"));
        var handler = new AddGroupCommandHandler(repository);

        await Assert.ThrowsAsync<GroupAlreadyExistsException>(() =>
            handler.HandleAsync(new AddGroupCommand("Students")));

        Assert.Single(repository.Groups);
        Assert.Equal(0, repository.SaveChangesCalls);
    }

    [Fact]
    public async Task AddGroupCommandHandler_runs_validator_before_repository_mutation()
    {
        var repository = new FakeGroupRepository();
        var handler = new AddGroupCommandHandler(repository);

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            handler.HandleAsync(new AddGroupCommand("Ad")));

        Assert.Contains("Name must be at least 3 characters long.", exception.Errors);
        Assert.Empty(repository.Groups);
        Assert.Equal(0, repository.SaveChangesCalls);
    }

    [Fact]
    public async Task GetAllGroupsQueryHandler_maps_groups_to_dtos()
    {
        var repository = new FakeGroupRepository();
        var userGroup = new Group("Students");
        SetGroupId(userGroup, 1);
        repository.Groups.Add(userGroup);
        var adminGroup = new Group("Admins");
        SetGroupId(adminGroup, 2);
        repository.Groups.Add(adminGroup);
        var handler = new GetAllGroupsQueryHandler(repository);

        var result = (await handler.HandleAsync(new GetAllGroupsQuery(1, 1))).ToList();

        var dto = Assert.Single(result);
        Assert.Equal(2, dto.Id);
        Assert.Equal("Admins", dto.Name);
        Assert.Equal((1, 1), repository.LastPaging);
    }

    private static void SetGroupId(Group group, int id)
    {
        typeof(Group)
            .GetProperty(nameof(Group.Id))!
            .SetValue(group, id);
    }

    private sealed class FakeGroupRepository : IGroupRepository
    {
        public List<Group> Groups { get; } = new();
        public int SaveChangesCalls { get; private set; }
        public (int Offset, int Limit)? LastPaging { get; private set; }

        public Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => Task.FromResult(Groups.FirstOrDefault(group => group.Id == id));

        public Task<Group?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
            => Task.FromResult(Groups.FirstOrDefault(group => group.Name == name));

        public Task<IEnumerable<Group>> GetAllAsync(int offset, int limit, CancellationToken cancellationToken = default)
        {
            LastPaging = (offset, limit);
            return Task.FromResult(Groups.Skip(offset).Take(limit).AsEnumerable());
        }

        public Task AddAsync(Group group, CancellationToken cancellationToken = default)
        {
            Groups.Add(group);
            return Task.CompletedTask;
        }

        public void UpdateAsync(Group group, CancellationToken cancellationToken = default)
        {
        }

        public void Delete(Group group)
        {
            Groups.Remove(group);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveChangesCalls++;
            return Task.CompletedTask;
        }
    }
}

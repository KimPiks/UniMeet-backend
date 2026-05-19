using ModularSystem.Contracts.UserEnrollment;
using ModularSystem.Contracts.UserEnrollment.UserAffiliations.AddAffiliation;
using ModularSystem.Contracts.UserEnrollment.UserAffiliations.GetAffiliationByUserId;
using UniMeet.UserEnrollmentModule.Application.UserAffiliations.AddAffiliation;
using UniMeet.UserEnrollmentModule.Application.UserAffiliations.GetAffiliationByUserId;
using UniMeet.UserEnrollmentModule.Application.UserAffiliations.GetFieldOfStudyIdsByUserIds;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UnitTests.Cqrs;

public class UserEnrollmentHandlersTests
{
    [Fact]
    public async Task AddAffiliationCommandHandler_adds_affiliation_when_user_has_none()
    {
        var repository = new FakeUserAffiliationRepository();
        var handler = new AddAffiliationCommandHandler(repository);
        var userId = Guid.NewGuid();

        await handler.HandleAsync(new AddAffiliationCommand(userId, 42));

        var affiliation = Assert.Single(repository.Affiliations);
        Assert.Equal(userId, affiliation.UserId);
        Assert.Equal(42, affiliation.FieldOfStudyId);
        Assert.Equal(1, repository.SaveChangesCalls);
    }

    [Fact]
    public async Task AddAffiliationCommandHandler_updates_existing_affiliation()
    {
        var userId = Guid.NewGuid();
        var repository = new FakeUserAffiliationRepository();
        repository.Affiliations.Add(new UserAffiliation(userId, 10) { Id = 3 });
        var handler = new AddAffiliationCommandHandler(repository);

        await handler.HandleAsync(new AddAffiliationCommand(userId, 99));

        var affiliation = Assert.Single(repository.Affiliations);
        Assert.Equal(3, affiliation.Id);
        Assert.Equal(99, affiliation.FieldOfStudyId);
        Assert.Equal(1, repository.SaveChangesCalls);
    }

    [Fact]
    public async Task GetAffiliationByUserIdQueryHandler_returns_null_when_affiliation_is_missing()
    {
        var repository = new FakeUserAffiliationRepository();
        var handler = new GetAffiliationByUserIdQueryHandler(repository);

        var result = await handler.HandleAsync(new GetAffiliationByUserIdQuery(Guid.NewGuid()));

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAffiliationByUserIdQueryHandler_maps_affiliation_to_dto()
    {
        var userId = Guid.NewGuid();
        var repository = new FakeUserAffiliationRepository();
        repository.Affiliations.Add(new UserAffiliation(userId, 12) { Id = 7 });
        var handler = new GetAffiliationByUserIdQueryHandler(repository);

        var result = await handler.HandleAsync(new GetAffiliationByUserIdQuery(userId));

        Assert.NotNull(result);
        Assert.Equal(7, result.Id);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(12, result.FieldOfStudyId);
    }

    [Fact]
    public async Task GetFieldOfStudyIdsByUserIdsQueryHandler_delegates_to_repository()
    {
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();
        var repository = new FakeUserAffiliationRepository();
        repository.Affiliations.Add(new UserAffiliation(userA, 1));
        repository.Affiliations.Add(new UserAffiliation(userB, 2));
        var handler = new GetFieldOfStudyIdsByUserIdsQueryHandler(repository);

        var result = await handler.HandleAsync(new GetFieldOfStudyIdsByUserIdsQuery([userA, userB]));

        Assert.Equal(1, result[userA]);
        Assert.Equal(2, result[userB]);
        Assert.Equal([userA, userB], repository.LastRequestedUserIds);
    }

    private sealed class FakeUserAffiliationRepository : IUserAffiliationRepository
    {
        public List<UserAffiliation> Affiliations { get; } = new();
        public List<Guid> LastRequestedUserIds { get; private set; } = new();
        public int SaveChangesCalls { get; private set; }

        public Task<UserAffiliation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => Task.FromResult(Affiliations.FirstOrDefault(affiliation => affiliation.Id == id));

        public Task<UserAffiliation?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
            => Task.FromResult(Affiliations.FirstOrDefault(affiliation => affiliation.UserId == userId));

        public Task<IReadOnlyList<Guid>> GetUserIdsByFieldOfStudyIdAsync(int fieldOfStudyId, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<Guid>>(
                Affiliations
                    .Where(affiliation => affiliation.FieldOfStudyId == fieldOfStudyId)
                    .Select(affiliation => affiliation.UserId)
                    .ToList());

        public Task<IDictionary<Guid, int>> GetFieldOfStudyIdsByUserIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
        {
            LastRequestedUserIds = userIds.ToList();
            var requested = LastRequestedUserIds.ToHashSet();
            return Task.FromResult<IDictionary<Guid, int>>(
                Affiliations
                    .Where(affiliation => requested.Contains(affiliation.UserId))
                    .ToDictionary(affiliation => affiliation.UserId, affiliation => affiliation.FieldOfStudyId));
        }

        public Task AddAsync(UserAffiliation userAffiliation, CancellationToken cancellationToken = default)
        {
            Affiliations.Add(userAffiliation);
            return Task.CompletedTask;
        }

        public void Delete(UserAffiliation userAffiliation, CancellationToken cancellationToken = default)
        {
            Affiliations.Remove(userAffiliation);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveChangesCalls++;
            return Task.CompletedTask;
        }
    }
}

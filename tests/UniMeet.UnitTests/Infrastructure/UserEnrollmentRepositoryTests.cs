using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;
using UniMeet.UserEnrollmentModule.Infrastructure.UserAffiliation;

namespace UniMeet.UnitTests.Infrastructure;

public class UserEnrollmentRepositoryTests
{
    [Fact]
    public async Task UserAffiliationRepository_adds_finds_queries_and_deletes_affiliation()
    {
        await using var context = RepositoryTestContextFactory.CreateUserEnrollmentContext();
        var repository = new UserAffiliationRepository(context);
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();
        var affiliationA = new UserAffiliation(userA, 5);
        var affiliationB = new UserAffiliation(userB, 5);

        await repository.AddAsync(affiliationA);
        await repository.AddAsync(affiliationB);
        await repository.SaveChangesAsync();
        var byId = await repository.GetByIdAsync(affiliationA.Id);
        var byUser = await repository.GetByUserIdAsync(userA);
        var usersByField = await repository.GetUserIdsByFieldOfStudyIdAsync(5);
        var fieldsByUsers = await repository.GetFieldOfStudyIdsByUserIdsAsync([userA, userA, userB]);
        var emptyFields = await repository.GetFieldOfStudyIdsByUserIdsAsync([]);
        repository.Delete(affiliationA);
        await repository.SaveChangesAsync();

        Assert.Same(affiliationA, byId);
        Assert.Same(affiliationA, byUser);
        Assert.Equal([userA, userB], usersByField);
        Assert.Equal(5, fieldsByUsers[userA]);
        Assert.Equal(5, fieldsByUsers[userB]);
        Assert.Empty(emptyFields);
        Assert.Null(await repository.GetByUserIdAsync(userA));
    }
}

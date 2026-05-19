using UniMeet.MatchingModule.Domain.Likes;
using UniMeet.MatchingModule.Domain.Matches;
using UniMeet.MatchingModule.Infrastructure.Likes;
using UniMeet.MatchingModule.Infrastructure.Matches;

namespace UniMeet.UnitTests.Infrastructure;

public class MatchingRepositoryTests
{
    [Fact]
    public async Task LikeRepository_adds_finds_orders_and_deletes_likes()
    {
        await using var context = RepositoryTestContextFactory.CreateMatchingContext();
        var repository = new LikeRepository(context);
        var likerId = Guid.NewGuid();
        var older = Like.Create(likerId, Guid.NewGuid());
        older.CreatedAt = DateTime.UtcNow.AddMinutes(-5);
        var newer = Like.Create(likerId, Guid.NewGuid());
        newer.CreatedAt = DateTime.UtcNow;

        await repository.AddAsync(older);
        await repository.AddAsync(newer);
        await repository.SaveChangesAsync();
        var found = await repository.GetAsync(likerId, newer.LikedId);
        var ordered = (await repository.GetByLikerIdAsync(likerId)).ToList();
        var exists = await repository.ExistsAsync(likerId, older.LikedId);
        repository.Delete(older);
        await repository.SaveChangesAsync();

        Assert.Same(newer, found);
        Assert.Equal([newer.Id, older.Id], ordered.Select(like => like.Id).ToArray());
        Assert.True(exists);
        Assert.False(await repository.ExistsAsync(likerId, older.LikedId));
    }

    [Fact]
    public async Task MatchRepository_normalizes_user_order_for_lookup_and_exists()
    {
        await using var context = RepositoryTestContextFactory.CreateMatchingContext();
        var repository = new MatchRepository(context);
        var lower = Guid.Parse("00000000-0000-0000-0000-000000000010");
        var higher = Guid.Parse("ffffffff-ffff-ffff-ffff-fffffffffff0");
        var match = Match.Create(higher, lower);

        await repository.AddAsync(match);
        await repository.SaveChangesAsync();
        var found = await repository.GetByUsersAsync(lower, higher);
        var existsReversed = await repository.ExistsAsync(higher, lower);
        var byUser = (await repository.GetByUserIdAsync(higher)).Single();
        repository.Delete(match);
        await repository.SaveChangesAsync();

        Assert.Same(match, found);
        Assert.True(existsReversed);
        Assert.Same(match, byUser);
        Assert.False(await repository.ExistsAsync(lower, higher));
    }
}

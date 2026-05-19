using ModularSystem.Contracts.Matching.Likes.LikeUser;
using ModularSystem.Contracts.Matching.Matches.Unmatch;
using UniMeet.MatchingModule.Application.Likes.LikeUser;
using UniMeet.MatchingModule.Application.Matches.Unmatch;
using UniMeet.MatchingModule.Domain.Likes;
using UniMeet.MatchingModule.Domain.Matches;
using UniMeet.MatchingModule.Infrastructure.Likes;
using UniMeet.MatchingModule.Infrastructure.Matches;

namespace UniMeet.UnitTests.Cqrs;

public class MatchingHandlersTests
{
    [Fact]
    public async Task LikeUserCommandHandler_records_first_like_without_match()
    {
        await using var context = RepositoryTestContextFactory.CreateMatchingContext();
        var likeRepository = new LikeRepository(context);
        var matchRepository = new MatchRepository(context);
        var handler = new LikeUserCommandHandler(likeRepository, matchRepository);
        var likerId = Guid.NewGuid();
        var likedId = Guid.NewGuid();

        var result = await handler.HandleAsync(new LikeUserCommand(likerId, likedId));

        Assert.False(result.Matched);
        Assert.False(result.JustMatched);
        Assert.True(await likeRepository.ExistsAsync(likerId, likedId));
        Assert.False(await matchRepository.ExistsAsync(likerId, likedId));
    }

    [Fact]
    public async Task LikeUserCommandHandler_creates_match_when_reverse_like_exists()
    {
        await using var context = RepositoryTestContextFactory.CreateMatchingContext();
        var likeRepository = new LikeRepository(context);
        var matchRepository = new MatchRepository(context);
        var handler = new LikeUserCommandHandler(likeRepository, matchRepository);
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();
        await likeRepository.AddAsync(Like.Create(userB, userA));
        await likeRepository.SaveChangesAsync();

        var result = await handler.HandleAsync(new LikeUserCommand(userA, userB));

        Assert.True(result.Matched);
        Assert.True(result.JustMatched);
        Assert.NotNull(result.MatchId);
        Assert.True(await matchRepository.ExistsAsync(userA, userB));
    }

    [Fact]
    public async Task LikeUserCommandHandler_returns_existing_match_without_new_like()
    {
        await using var context = RepositoryTestContextFactory.CreateMatchingContext();
        var likeRepository = new LikeRepository(context);
        var matchRepository = new MatchRepository(context);
        var handler = new LikeUserCommandHandler(likeRepository, matchRepository);
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();
        var match = Match.Create(userA, userB);
        await matchRepository.AddAsync(match);
        await matchRepository.SaveChangesAsync();

        var result = await handler.HandleAsync(new LikeUserCommand(userA, userB));

        Assert.True(result.Matched);
        Assert.False(result.JustMatched);
        Assert.Equal(match.Id, result.MatchId);
        Assert.False(await likeRepository.ExistsAsync(userA, userB));
    }

    [Fact]
    public async Task UnmatchCommandHandler_removes_match_and_both_likes_when_they_exist()
    {
        await using var context = RepositoryTestContextFactory.CreateMatchingContext();
        var likeRepository = new LikeRepository(context);
        var matchRepository = new MatchRepository(context);
        var handler = new UnmatchCommandHandler(matchRepository, likeRepository);
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();
        await matchRepository.AddAsync(Match.Create(userA, userB));
        await likeRepository.AddAsync(Like.Create(userA, userB));
        await likeRepository.AddAsync(Like.Create(userB, userA));
        await matchRepository.SaveChangesAsync();

        await handler.HandleAsync(new UnmatchCommand(userA, userB));

        Assert.False(await matchRepository.ExistsAsync(userA, userB));
        Assert.False(await likeRepository.ExistsAsync(userA, userB));
        Assert.False(await likeRepository.ExistsAsync(userB, userA));
    }
}

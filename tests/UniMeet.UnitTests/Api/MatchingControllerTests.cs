using ModularSystem.Contracts.Matching.Likes;
using ModularSystem.Contracts.Matching.Likes.LikeUser;
using ModularSystem.Contracts.Messaging.Conversations.CreateConversation;
using UniMeet.API.Controllers.Matching;

namespace UniMeet.UnitTests.Api;

public class MatchingControllerTests
{
    [Fact]
    public async Task LikeUser_When_just_matched_creates_private_conversation()
    {
        var currentUserId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var dispatcher = new FakeModuleRequestDispatcher();
        dispatcher.QueueResult(new LikeResultDto(true, matchId, true));
        dispatcher.QueueResult(new ModularSystem.Contracts.Messaging.Conversations.ConversationDto(
            Guid.NewGuid(),
            false,
            currentUserId,
            otherUserId,
            [currentUserId, otherUserId],
            DateTime.UtcNow,
            null));
        var controller = new MatchingController(dispatcher);
        controller.SetCurrentUser(currentUserId);

        var result = await controller.LikeUser(new LikeUserRequest(otherUserId));

        var response = ControllerTestHelpers.AssertOkResponse<LikeResultDto>(result, "It's a match!");
        Assert.True(response.Data.Matched);
        Assert.Collection(
            dispatcher.SentRequests,
            request =>
            {
                var command = Assert.IsType<LikeUserCommand>(request);
                Assert.Equal(currentUserId, command.LikerId);
                Assert.Equal(otherUserId, command.LikedId);
            },
            request =>
            {
                var command = Assert.IsType<CreateConversationCommand>(request);
                Assert.Equal(currentUserId, command.UserAId);
                Assert.Equal(otherUserId, command.UserBId);
            });
    }

    [Fact]
    public async Task LikeUser_When_like_is_recorded_does_not_create_conversation()
    {
        var currentUserId = Guid.NewGuid();
        var dispatcher = new FakeModuleRequestDispatcher();
        dispatcher.QueueResult(new LikeResultDto(false, null, false));
        var controller = new MatchingController(dispatcher);
        controller.SetCurrentUser(currentUserId);

        var result = await controller.LikeUser(new LikeUserRequest(Guid.NewGuid()));

        var response = ControllerTestHelpers.AssertOkResponse<LikeResultDto>(result, "Like recorded");
        Assert.False(response.Data.Matched);
        Assert.Single(dispatcher.SentRequests);
    }
}

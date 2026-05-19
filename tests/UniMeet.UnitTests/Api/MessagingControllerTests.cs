using ModularSystem.Contracts.Matching.Matches.AreUsersMatched;
using ModularSystem.Contracts.Messaging.Conversations;
using ModularSystem.Contracts.Messaging.Conversations.CreateGroupConversation;
using ModularSystem.Contracts.Messaging.Conversations.GetConversationById;
using ModularSystem.Contracts.Messaging.Messages;
using ModularSystem.Contracts.Messaging.Messages.SendMessage;
using UniMeet.API.Controllers.Messaging;
using UniMeet.Shared.Exceptions;

namespace UniMeet.UnitTests.Api;

public class MessagingControllerTests
{
    [Fact]
    public async Task GetConversation_When_users_are_not_matched_throws_forbidden_exception()
    {
        var dispatcher = new FakeModuleRequestDispatcher();
        dispatcher.QueueResult(false);
        var controller = new MessagingController(dispatcher);
        controller.SetCurrentUser(Guid.NewGuid());

        await Assert.ThrowsAsync<ForbiddenException>(() => controller.GetConversation(Guid.NewGuid()));

        Assert.IsType<AreUsersMatchedQuery>(dispatcher.SentRequests.Single());
    }

    [Fact]
    public async Task CreateGroupConversation_Rejects_duplicate_participants_before_module_calls()
    {
        var participantId = Guid.NewGuid();
        var dispatcher = new FakeModuleRequestDispatcher();
        var controller = new MessagingController(dispatcher);
        controller.SetCurrentUser(Guid.NewGuid());

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            controller.CreateGroupConversation(new CreateGroupConversationRequest([participantId, participantId])));

        Assert.Contains("participantIds cannot contain duplicates.", exception.Errors);
        Assert.Empty(dispatcher.SentRequests);
    }

    [Fact]
    public async Task CreateGroupConversation_Checks_matches_and_sends_create_command()
    {
        var userId = Guid.NewGuid();
        var participantA = Guid.NewGuid();
        var participantB = Guid.NewGuid();
        var conversation = new ConversationDto(
            Guid.NewGuid(),
            true,
            null,
            null,
            [userId, participantA, participantB],
            DateTime.UtcNow,
            null);
        var dispatcher = new FakeModuleRequestDispatcher();
        dispatcher.QueueResult(true);
        dispatcher.QueueResult(true);
        dispatcher.QueueResult(conversation);
        var controller = new MessagingController(dispatcher);
        controller.SetCurrentUser(userId);

        var result = await controller.CreateGroupConversation(new CreateGroupConversationRequest([participantA, participantB]));

        var response = ControllerTestHelpers.AssertOkResponse<ConversationDto>(result, "Group conversation created");
        Assert.Same(conversation, response.Data);
        Assert.Collection(
            dispatcher.SentRequests,
            request => Assert.IsType<AreUsersMatchedQuery>(request),
            request => Assert.IsType<AreUsersMatchedQuery>(request),
            request =>
            {
                var command = Assert.IsType<CreateGroupConversationCommand>(request);
                Assert.Equal(userId, command.CreatorUserId);
                Assert.Equal([participantA, participantB], command.ParticipantIds);
            });
    }

    [Fact]
    public async Task SendMessage_In_private_conversation_requires_match_and_sends_message_command()
    {
        var currentUserId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var conversationId = Guid.NewGuid();
        var conversation = new ConversationDto(
            conversationId,
            false,
            currentUserId,
            otherUserId,
            [currentUserId, otherUserId],
            DateTime.UtcNow,
            null);
        var message = new MessageDto(Guid.NewGuid(), conversationId, currentUserId, "hello", DateTime.UtcNow, false);
        var dispatcher = new FakeModuleRequestDispatcher();
        dispatcher.QueueResult(conversation);
        dispatcher.QueueResult(true);
        dispatcher.QueueResult(message);
        var controller = new MessagingController(dispatcher);
        controller.SetCurrentUser(currentUserId);

        var result = await controller.SendMessage(new SendMessageRequest(conversationId, "hello"));

        var response = ControllerTestHelpers.AssertOkResponse<MessageDto>(result, "Message sent");
        Assert.Same(message, response.Data);
        Assert.Collection(
            dispatcher.SentRequests,
            request => Assert.IsType<GetConversationByIdQuery>(request),
            request => Assert.IsType<AreUsersMatchedQuery>(request),
            request =>
            {
                var command = Assert.IsType<SendMessageCommand>(request);
                Assert.Equal(currentUserId, command.SenderId);
                Assert.Equal(conversationId, command.ConversationId);
                Assert.Equal("hello", command.Content);
            });
    }
}

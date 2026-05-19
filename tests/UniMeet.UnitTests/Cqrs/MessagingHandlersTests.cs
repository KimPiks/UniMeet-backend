using ModularSystem.Contracts.Messaging.Conversations.CreateConversation;
using ModularSystem.Contracts.Messaging.Conversations.CreateGroupConversation;
using ModularSystem.Contracts.Messaging.Messages;
using ModularSystem.Contracts.Messaging.Messages.MarkMessagesAsRead;
using ModularSystem.Contracts.Messaging.Messages.SendMessage;
using UniMeet.MessagingModule.Application.Conversations.CreateConversation;
using UniMeet.MessagingModule.Application.Conversations.CreateGroupConversation;
using UniMeet.MessagingModule.Application.Messages.MarkMessagesAsRead;
using UniMeet.MessagingModule.Application.Messages.SendMessage;
using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.MessagingModule.Infrastructure.Conversations;
using UniMeet.MessagingModule.Infrastructure.Messages;
using UniMeet.Shared.Exceptions;

namespace UniMeet.UnitTests.Cqrs;

public class MessagingHandlersTests
{
    [Fact]
    public async Task CreateConversationCommandHandler_returns_existing_conversation_when_present()
    {
        await using var context = RepositoryTestContextFactory.CreateMessagingContext();
        var repository = new ConversationRepository(context);
        var userA = Guid.NewGuid();
        var userB = Guid.NewGuid();
        var existing = Conversation.Create(userA, userB);
        await repository.AddAsync(existing);
        await repository.SaveChangesAsync();
        var handler = new CreateConversationCommandHandler(repository);

        var result = await handler.HandleAsync(new CreateConversationCommand(userA, userB));

        Assert.Equal(existing.Id, result.Id);
        Assert.Single(context.Conversations);
    }

    [Fact]
    public async Task CreateGroupConversationCommandHandler_validates_participants_and_creates_group()
    {
        await using var context = RepositoryTestContextFactory.CreateMessagingContext();
        var repository = new ConversationRepository(context);
        var handler = new CreateGroupConversationCommandHandler(repository);
        var creatorId = Guid.NewGuid();
        var participantA = Guid.NewGuid();
        var participantB = Guid.NewGuid();

        var result = await handler.HandleAsync(new CreateGroupConversationCommand(creatorId, [participantA, participantB]));

        Assert.True(result.IsGroup);
        Assert.Equal(new[] { creatorId, participantA, participantB }.Order().ToArray(), result.ParticipantIds.ToArray());
        Assert.Single(context.Conversations);
    }

    [Fact]
    public async Task CreateGroupConversationCommandHandler_rejects_duplicate_participants()
    {
        await using var context = RepositoryTestContextFactory.CreateMessagingContext();
        var handler = new CreateGroupConversationCommandHandler(new ConversationRepository(context));
        var participantId = Guid.NewGuid();

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            handler.HandleAsync(new CreateGroupConversationCommand(Guid.NewGuid(), [participantId, participantId])));

        Assert.Contains("participantIds cannot contain duplicates.", exception.Errors);
    }

    [Fact]
    public async Task SendMessageCommandHandler_persists_message_and_notifies_recipients()
    {
        await using var context = RepositoryTestContextFactory.CreateMessagingContext();
        var conversationRepository = new ConversationRepository(context);
        var messageRepository = new MessageRepository(context);
        var notifier = new CapturingMessagingHubNotifier();
        var senderId = Guid.NewGuid();
        var recipientId = Guid.NewGuid();
        var conversation = Conversation.Create(senderId, recipientId);
        await conversationRepository.AddAsync(conversation);
        await conversationRepository.SaveChangesAsync();
        var handler = new SendMessageCommandHandler(conversationRepository, messageRepository, notifier);

        var result = await handler.HandleAsync(new SendMessageCommand(senderId, conversation.Id, "hello"));

        Assert.Equal("hello", result.Content);
        Assert.True(result.IsRead);
        Assert.Single(context.Messages);
        Assert.Equal([recipientId], notifier.RecipientUserIds);
        Assert.False(notifier.Message!.IsRead);
    }

    [Fact]
    public async Task SendMessageCommandHandler_rejects_non_participant_sender()
    {
        await using var context = RepositoryTestContextFactory.CreateMessagingContext();
        var conversationRepository = new ConversationRepository(context);
        var conversation = Conversation.Create(Guid.NewGuid(), Guid.NewGuid());
        await conversationRepository.AddAsync(conversation);
        await conversationRepository.SaveChangesAsync();
        var handler = new SendMessageCommandHandler(
            conversationRepository,
            new MessageRepository(context),
            new CapturingMessagingHubNotifier());

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            handler.HandleAsync(new SendMessageCommand(Guid.NewGuid(), conversation.Id, "hello")));
    }

    [Fact]
    public async Task MarkMessagesAsReadCommandHandler_creates_read_receipts_for_unread_messages()
    {
        await using var context = RepositoryTestContextFactory.CreateMessagingContext();
        var conversationRepository = new ConversationRepository(context);
        var messageRepository = new MessageRepository(context);
        var senderId = Guid.NewGuid();
        var recipientId = Guid.NewGuid();
        var conversation = Conversation.Create(senderId, recipientId);
        await conversationRepository.AddAsync(conversation);
        var unread = Message.Create(conversation.Id, senderId, "unread");
        await messageRepository.AddAsync(unread);
        await messageRepository.SaveChangesAsync();
        var handler = new MarkMessagesAsReadCommandHandler(conversationRepository, messageRepository);

        await handler.HandleAsync(new MarkMessagesAsReadCommand(conversation.Id, recipientId));

        var receipt = Assert.Single(context.MessageReadReceipts);
        Assert.Equal(unread.Id, receipt.MessageId);
        Assert.Equal(recipientId, receipt.UserId);
    }

    private sealed class CapturingMessagingHubNotifier : IMessagingHubNotifier
    {
        public Guid[] RecipientUserIds { get; private set; } = [];
        public MessageDto? Message { get; private set; }

        public Task SendMessageAsync(IEnumerable<Guid> recipientUserIds, MessageDto message, CancellationToken cancellationToken = default)
        {
            RecipientUserIds = recipientUserIds.ToArray();
            Message = message;
            return Task.CompletedTask;
        }
    }
}

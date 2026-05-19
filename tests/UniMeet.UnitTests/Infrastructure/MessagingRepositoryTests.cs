using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.MessagingModule.Infrastructure.Conversations;
using UniMeet.MessagingModule.Infrastructure.Messages;

namespace UniMeet.UnitTests.Infrastructure;

public class MessagingRepositoryTests
{
    [Fact]
    public async Task ConversationRepository_finds_private_conversation_by_users_and_participant()
    {
        await using var context = RepositoryTestContextFactory.CreateMessagingContext();
        var repository = new ConversationRepository(context);
        var userA = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var userB = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        var conversation = Conversation.Create(userB, userA);

        await repository.AddAsync(conversation);
        await repository.SaveChangesAsync();

        var byId = await repository.GetByIdAsync(conversation.Id);
        var byUsers = await repository.GetByUsersAsync(userA, userB);
        var isParticipant = await repository.IsParticipantAsync(conversation.Id, userA);

        Assert.NotNull(byId);
        Assert.Equal(2, byId.Participants.Count);
        Assert.Same(conversation, byUsers);
        Assert.True(isParticipant);
    }

    [Fact]
    public async Task ConversationRepository_orders_user_conversations_by_last_message_then_created_at()
    {
        await using var context = RepositoryTestContextFactory.CreateMessagingContext();
        var repository = new ConversationRepository(context);
        var userId = Guid.NewGuid();
        var otherA = Guid.NewGuid();
        var otherB = Guid.NewGuid();
        var olderConversation = Conversation.Create(userId, otherA);
        olderConversation.CreatedAt = DateTime.UtcNow.AddDays(-2);
        var newerConversation = Conversation.Create(userId, otherB);
        newerConversation.CreatedAt = DateTime.UtcNow.AddDays(-1);
        var message = Message.Create(newerConversation.Id, otherB, "latest");
        message.SentAt = DateTime.UtcNow;
        newerConversation.Messages.Add(message);

        await repository.AddAsync(olderConversation);
        await repository.AddAsync(newerConversation);
        await repository.SaveChangesAsync();

        var conversations = (await repository.GetByUserIdAsync(userId)).ToList();

        Assert.Equal([newerConversation.Id, olderConversation.Id], conversations.Select(c => c.Id).ToArray());
    }

    [Fact]
    public async Task MessageRepository_paginates_chronologically_and_filters_unread_messages()
    {
        await using var context = RepositoryTestContextFactory.CreateMessagingContext();
        var repository = new MessageRepository(context);
        var conversationId = Guid.NewGuid();
        var senderId = Guid.NewGuid();
        var recipientId = Guid.NewGuid();
        var oldest = Message.Create(conversationId, senderId, "oldest");
        oldest.SentAt = DateTime.UtcNow.AddMinutes(-3);
        var middle = Message.Create(conversationId, senderId, "middle");
        middle.SentAt = DateTime.UtcNow.AddMinutes(-2);
        var newest = Message.Create(conversationId, recipientId, "newest");
        newest.SentAt = DateTime.UtcNow.AddMinutes(-1);
        middle.ReadReceipts.Add(MessageReadReceipt.Create(middle.Id, recipientId));

        await repository.AddAsync(oldest);
        await repository.AddAsync(middle);
        await repository.AddAsync(newest);
        await repository.SaveChangesAsync();

        var page = (await repository.GetByConversationIdAsync(conversationId, 0, 2)).ToList();
        var unread = (await repository.GetUnreadMessageIdsByConversationAndRecipientAsync(conversationId, recipientId)).ToList();

        Assert.Equal([middle.Id, newest.Id], page.Select(message => message.Id).ToArray());
        Assert.Equal([oldest.Id], unread);
    }
}

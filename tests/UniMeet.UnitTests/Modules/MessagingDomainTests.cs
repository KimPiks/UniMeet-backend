using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.UnitTests.Modules;

public class MessagingDomainTests
{
    [Fact]
    public void Private_conversation_orders_users_and_adds_two_participants()
    {
        var higher = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        var lower = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var conversation = Conversation.Create(higher, lower);

        Assert.False(conversation.IsGroup);
        Assert.Equal(lower, conversation.User1Id);
        Assert.Equal(higher, conversation.User2Id);
        Assert.Equal([lower, higher], conversation.Participants.Select(participant => participant.UserId).Order().ToArray());
    }

    [Fact]
    public void Private_conversation_rejects_same_user()
    {
        var userId = Guid.NewGuid();

        var exception = Assert.Throws<InvalidOperationException>(() => Conversation.Create(userId, userId));

        Assert.Equal("A user cannot create a private conversation with themselves.", exception.Message);
    }

    [Fact]
    public void Group_conversation_adds_creator_once_and_requires_three_total_participants()
    {
        var creator = Guid.NewGuid();
        var participantA = Guid.NewGuid();
        var participantB = Guid.NewGuid();

        var conversation = Conversation.CreateGroup(creator, [participantA, participantB, participantA]);

        Assert.True(conversation.IsGroup);
        Assert.Equal(creator, conversation.CreatedByUserId);
        Assert.Equal(3, conversation.Participants.Count);
        Assert.Contains(conversation.Participants, participant => participant.UserId == creator);
        Assert.Contains(conversation.Participants, participant => participant.UserId == participantA);
        Assert.Contains(conversation.Participants, participant => participant.UserId == participantB);
    }

    [Fact]
    public void Group_conversation_rejects_less_than_three_total_participants()
    {
        var creator = Guid.NewGuid();
        var participant = Guid.NewGuid();

        var exception = Assert.Throws<InvalidOperationException>(() => Conversation.CreateGroup(creator, [participant]));

        Assert.Equal("A group conversation requires at least three participants.", exception.Message);
    }

    [Fact]
    public void Message_Create_sets_identifiers_content_and_timestamp()
    {
        var conversationId = Guid.NewGuid();
        var senderId = Guid.NewGuid();

        var message = Message.Create(conversationId, senderId, "hello");

        Assert.NotEqual(Guid.Empty, message.Id);
        Assert.Equal(conversationId, message.ConversationId);
        Assert.Equal(senderId, message.SenderId);
        Assert.Equal("hello", message.Content);
        Assert.True(message.SentAt <= DateTime.UtcNow);
    }
}

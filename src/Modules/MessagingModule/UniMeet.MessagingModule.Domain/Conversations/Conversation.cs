using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.MessagingModule.Domain.Conversations;

public class Conversation
{
    public Guid Id { get; set; }

    public bool IsGroup { get; set; }
    public Guid? CreatedByUserId { get; set; }

    // For private conversations, User1Id is always the smaller GUID to enforce uniqueness regardless of order.
    public Guid? User1Id { get; set; }
    public Guid? User2Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Message> Messages { get; set; } = new();
    public List<ConversationParticipant> Participants { get; set; } = new();

    private Conversation() { }

    public static Conversation Create(Guid userA, Guid userB)
    {
        if (userA == userB)
            throw new InvalidOperationException("A user cannot create a private conversation with themselves.");

        var (user1, user2) = userA.CompareTo(userB) < 0 ? (userA, userB) : (userB, userA);
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            IsGroup = false,
            User1Id = user1,
            User2Id = user2,
            CreatedAt = DateTime.UtcNow
        };

        conversation.Participants.Add(ConversationParticipant.Create(conversation.Id, user1));
        conversation.Participants.Add(ConversationParticipant.Create(conversation.Id, user2));

        return conversation;
    }

    public static Conversation CreateGroup(Guid creatorUserId, IEnumerable<Guid> invitedUserIds)
    {
        var participantIds = invitedUserIds
            .Append(creatorUserId)
            .Distinct()
            .ToArray();

        if (participantIds.Length < 3)
            throw new InvalidOperationException("A group conversation requires at least three participants.");

        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            IsGroup = true,
            CreatedByUserId = creatorUserId,
            CreatedAt = DateTime.UtcNow
        };

        conversation.Participants.AddRange(participantIds.Select(id => ConversationParticipant.Create(conversation.Id, id)));

        return conversation;
    }
}

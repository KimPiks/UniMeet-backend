using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.MessagingModule.Domain.Conversations;

public class Conversation
{
    public Guid Id { get; set; }

    // User1Id is always the smaller GUID to enforce uniqueness regardless of order
    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Message> Messages { get; set; } = new();

    private Conversation() { }

    public static Conversation Create(Guid userA, Guid userB)
    {
        var (user1, user2) = userA.CompareTo(userB) < 0 ? (userA, userB) : (userB, userA);
        return new Conversation
        {
            Id = Guid.NewGuid(),
            User1Id = user1,
            User2Id = user2,
            CreatedAt = DateTime.UtcNow
        };
    }
}

namespace UniMeet.MessagingModule.Domain.Conversations;

public class ConversationParticipant
{
    public Guid ConversationId { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinedAt { get; set; }

    private ConversationParticipant() { }

    public static ConversationParticipant Create(Guid conversationId, Guid userId)
    {
        return new ConversationParticipant
        {
            ConversationId = conversationId,
            UserId = userId,
            JoinedAt = DateTime.UtcNow
        };
    }
}

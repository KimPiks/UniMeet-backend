namespace UniMeet.MessagingModule.Domain.Messages;

public class Message
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }

    private Message() { }

    public static Message Create(Guid conversationId, Guid senderId, string content)
    {
        return new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}

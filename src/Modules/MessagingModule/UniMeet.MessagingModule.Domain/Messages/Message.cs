namespace UniMeet.MessagingModule.Domain.Messages;

public class Message
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }
    public List<MessageReadReceipt> ReadReceipts { get; set; } = new();

    private Message() { }

    public static Message Create(Guid conversationId, Guid senderId, string content)
    {
        return new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.UtcNow
        };
    }
}

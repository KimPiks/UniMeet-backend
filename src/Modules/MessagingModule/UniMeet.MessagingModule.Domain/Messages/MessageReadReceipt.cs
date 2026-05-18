namespace UniMeet.MessagingModule.Domain.Messages;

public class MessageReadReceipt
{
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public DateTime ReadAt { get; set; }

    private MessageReadReceipt() { }

    public static MessageReadReceipt Create(Guid messageId, Guid userId)
    {
        return new MessageReadReceipt
        {
            MessageId = messageId,
            UserId = userId,
            ReadAt = DateTime.UtcNow
        };
    }
}

namespace UniMeet.MessagingModule.Domain.Messages;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetByConversationIdAsync(Guid conversationId, int offset, int limit, CancellationToken cancellationToken = default);
    Task<IEnumerable<Guid>> GetUnreadMessageIdsByConversationAndRecipientAsync(Guid conversationId, Guid recipientId, CancellationToken cancellationToken = default);
    Task AddAsync(Message message, CancellationToken cancellationToken = default);
    Task AddReadReceiptsAsync(IEnumerable<MessageReadReceipt> readReceipts, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

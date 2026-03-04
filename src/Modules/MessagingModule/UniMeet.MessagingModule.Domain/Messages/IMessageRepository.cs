namespace UniMeet.MessagingModule.Domain.Messages;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetByConversationIdAsync(Guid conversationId, int offset, int limit, CancellationToken cancellationToken = default);
    Task<IEnumerable<Message>> GetUnreadByConversationAndRecipientAsync(Guid conversationId, Guid recipientId, CancellationToken cancellationToken = default);
    Task AddAsync(Message message, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

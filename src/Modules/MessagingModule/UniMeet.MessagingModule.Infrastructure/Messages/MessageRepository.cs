using Microsoft.EntityFrameworkCore;
using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.MessagingModule.Infrastructure.Messages;

public class MessageRepository(MessagingContext context) : IMessageRepository
{
    public async Task<IEnumerable<Message>> GetByConversationIdAsync(Guid conversationId, int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.Messages
            .Include(m => m.ReadReceipts)
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.SentAt)
            .Skip(offset)
            .Take(limit)
            .OrderBy(m => m.SentAt) // re-order chronologically for the caller
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Guid>> GetUnreadMessageIdsByConversationAndRecipientAsync(Guid conversationId, Guid recipientId, CancellationToken cancellationToken = default)
    {
        return await context.Messages
            .Where(m => m.ConversationId == conversationId &&
                        m.SenderId != recipientId &&
                        !m.ReadReceipts.Any(r => r.UserId == recipientId))
            .Select(m => m.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Message message, CancellationToken cancellationToken = default)
    {
        await context.Messages.AddAsync(message, cancellationToken);
    }

    public async Task AddReadReceiptsAsync(IEnumerable<MessageReadReceipt> readReceipts, CancellationToken cancellationToken = default)
    {
        await context.MessageReadReceipts.AddRangeAsync(readReceipts, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.MessagingModule.Infrastructure.Messages;

public class MessageRepository(MessagingContext context) : IMessageRepository
{
    public async Task<IEnumerable<Message>> GetByConversationIdAsync(Guid conversationId, int offset, int limit, CancellationToken cancellationToken = default)
    {
        return await context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.SentAt)
            .Skip(offset)
            .Take(limit)
            .OrderBy(m => m.SentAt) // re-order chronologically for the caller
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Message>> GetUnreadByConversationAndRecipientAsync(Guid conversationId, Guid recipientId, CancellationToken cancellationToken = default)
    {
        return await context.Messages
            .Where(m => m.ConversationId == conversationId && m.SenderId != recipientId && !m.IsRead)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Message message, CancellationToken cancellationToken = default)
    {
        await context.Messages.AddAsync(message, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

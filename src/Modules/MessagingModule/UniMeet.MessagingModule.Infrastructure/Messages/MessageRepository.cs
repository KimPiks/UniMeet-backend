using Microsoft.EntityFrameworkCore;
using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.MessagingModule.Infrastructure.Messages;

public class MessageRepository(MessagingContext context) : IMessageRepository
{
    private const int DefaultPageSize = 50;
    private const int MaxPageSize = 100;

    public async Task<IEnumerable<Message>> GetByConversationIdAsync(Guid conversationId, int offset, int limit, CancellationToken cancellationToken = default)
    {
        var safeOffset = Math.Max(0, offset);
        var safeLimit = limit <= 0 ? DefaultPageSize : Math.Min(limit, MaxPageSize);
        var page = await context.Messages
            .AsNoTracking()
            .Include(m => m.ReadReceipts)
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.SentAt)
            .Skip(safeOffset)
            .Take(safeLimit)
            .ToListAsync(cancellationToken);

        return page.OrderBy(m => m.SentAt).ToList();
    }

    public async Task<IEnumerable<Guid>> GetUnreadMessageIdsByConversationAndRecipientAsync(Guid conversationId, Guid recipientId, CancellationToken cancellationToken = default)
    {
        return await context.Messages
            .AsNoTracking()
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

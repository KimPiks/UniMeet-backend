using Microsoft.EntityFrameworkCore;
using UniMeet.MessagingModule.Domain.Conversations;

namespace UniMeet.MessagingModule.Infrastructure.Conversations;

public class ConversationRepository(MessagingContext context) : IConversationRepository
{
    public async Task<Conversation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Conversation?> GetByUsersAsync(Guid userA, Guid userB, CancellationToken cancellationToken = default)
    {
        var (user1, user2) = userA.CompareTo(userB) < 0 ? (userA, userB) : (userB, userA);
        return await context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.User1Id == user1 && c.User2Id == user2, cancellationToken);
    }

    public async Task<IEnumerable<Conversation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Conversations
            .Include(c => c.Messages)
            .Where(c => c.User1Id == userId || c.User2Id == userId)
            .OrderByDescending(c => c.Messages.Max(m => (DateTime?)m.SentAt) ?? c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Conversation conversation, CancellationToken cancellationToken = default)
    {
        await context.Conversations.AddAsync(conversation, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}

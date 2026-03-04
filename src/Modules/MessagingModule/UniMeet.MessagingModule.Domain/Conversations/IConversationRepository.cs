namespace UniMeet.MessagingModule.Domain.Conversations;

public interface IConversationRepository
{
    Task<Conversation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Conversation?> GetByUsersAsync(Guid userA, Guid userB, CancellationToken cancellationToken = default);
    Task<IEnumerable<Conversation>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Conversation conversation, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

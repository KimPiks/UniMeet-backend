using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.GetUserConversations;

public class GetUserConversationsQueryHandler(IConversationRepository conversationRepository)
    : IQueryHandler<GetUserConversationsQuery, IEnumerable<ConversationDto>>
{
    public async Task<IEnumerable<ConversationDto>> HandleAsync(GetUserConversationsQuery request, CancellationToken cancellationToken = default)
    {
        var conversations = await conversationRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        return conversations.Select(c =>
        {
            var last = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
            var lastDto = last is null
                ? null
                : new MessageSummaryDto(last.SenderId, last.Content, last.SentAt, last.IsRead);
            return new ConversationDto(c.Id, c.User1Id, c.User2Id, c.CreatedAt, lastDto);
        });
    }
}

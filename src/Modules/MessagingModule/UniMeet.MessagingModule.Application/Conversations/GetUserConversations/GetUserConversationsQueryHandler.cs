using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.GetUserConversations;

public class GetUserConversationsQueryHandler(IConversationRepository conversationRepository)
    : IQueryHandler<GetUserConversationsQuery, IEnumerable<ConversationDto>>
{
    public async Task<IEnumerable<ConversationDto>> HandleAsync(GetUserConversationsQuery request, CancellationToken cancellationToken = default)
    {
        var conversations = await conversationRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        return conversations.Select(c => c.ToDto(request.UserId));
    }
}

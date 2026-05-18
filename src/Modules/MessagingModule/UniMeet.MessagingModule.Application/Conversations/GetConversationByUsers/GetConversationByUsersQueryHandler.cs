using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.GetConversationByUsers;

public class GetConversationByUsersQueryHandler(IConversationRepository conversationRepository)
    : IQueryHandler<GetConversationByUsersQuery, ConversationDto?>
{
    public async Task<ConversationDto?> HandleAsync(GetConversationByUsersQuery request, CancellationToken cancellationToken = default)
    {
        var conversation = await conversationRepository.GetByUsersAsync(request.UserAId, request.UserBId, cancellationToken);
        if (conversation is null)
            return null;

        return conversation.ToDto(request.UserAId);
    }
}

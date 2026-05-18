using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.GetConversationById;

public class GetConversationByIdQueryHandler(IConversationRepository conversationRepository)
    : IQueryHandler<GetConversationByIdQuery, ConversationDto?>
{
    public async Task<ConversationDto?> HandleAsync(GetConversationByIdQuery request, CancellationToken cancellationToken = default)
    {
        var conversation = await conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation is null)
            return null;

        return conversation.ToDto(request.CurrentUserId);
    }
}

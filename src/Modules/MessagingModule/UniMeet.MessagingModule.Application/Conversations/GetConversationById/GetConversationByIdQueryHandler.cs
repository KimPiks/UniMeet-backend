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

        var last = conversation.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
        var lastDto = last is null
            ? null
            : new MessageSummaryDto(last.SenderId, last.Content, last.SentAt, last.IsRead);
        return new ConversationDto(conversation.Id, conversation.User1Id, conversation.User2Id, conversation.CreatedAt, lastDto);
    }
}

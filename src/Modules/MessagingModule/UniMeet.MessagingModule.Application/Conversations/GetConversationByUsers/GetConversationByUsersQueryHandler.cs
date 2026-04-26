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

        var last = conversation.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
        var lastDto = last is null
            ? null
            : new MessageSummaryDto(last.SenderId, last.Content, last.SentAt, last.IsRead);
        return new ConversationDto(conversation.Id, conversation.User1Id, conversation.User2Id, conversation.CreatedAt, lastDto);
    }
}

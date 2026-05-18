using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Exceptions;

namespace UniMeet.MessagingModule.Application.Messages.GetConversationMessages;

public class GetConversationMessagesQueryHandler(
    IConversationRepository conversationRepository,
    IMessageRepository messageRepository)
    : IQueryHandler<GetConversationMessagesQuery, IEnumerable<MessageDto>>
{
    public async Task<IEnumerable<MessageDto>> HandleAsync(GetConversationMessagesQuery request, CancellationToken cancellationToken = default)
    {
        var isParticipant = await conversationRepository.IsParticipantAsync(request.ConversationId, request.CurrentUserId, cancellationToken);
        if (!isParticipant)
            throw new ForbiddenException("You are not a participant of this conversation.");

        var messages = await messageRepository.GetByConversationIdAsync(
            request.ConversationId, request.Offset, request.Limit, cancellationToken);

        return messages.Select(m => m.ToDto(request.CurrentUserId));
    }
}

using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Messages.GetConversationMessages;

public class GetConversationMessagesQueryHandler(IMessageRepository messageRepository)
    : IQueryHandler<GetConversationMessagesQuery, IEnumerable<MessageDto>>
{
    public async Task<IEnumerable<MessageDto>> HandleAsync(GetConversationMessagesQuery request, CancellationToken cancellationToken = default)
    {
        var messages = await messageRepository.GetByConversationIdAsync(
            request.ConversationId, request.Offset, request.Limit, cancellationToken);

        return messages.Select(m => new MessageDto(m.Id, m.ConversationId, m.SenderId, m.Content, m.SentAt, m.IsRead));
    }
}

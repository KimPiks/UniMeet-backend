using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.GetOrCreateConversation;

public class GetOrCreateConversationCommandHandler(IConversationRepository conversationRepository)
    : ICommandHandler<GetOrCreateConversationCommand, ConversationDto>
{
    public async Task<ConversationDto> HandleAsync(GetOrCreateConversationCommand request, CancellationToken cancellationToken = default)
    {
        var existing = await conversationRepository.GetByUsersAsync(
            request.RequestingUserId, request.OtherUserId, cancellationToken);

        if (existing != null)
            return MapToDto(existing);

        var conversation = Conversation.Create(request.RequestingUserId, request.OtherUserId);
        await conversationRepository.AddAsync(conversation, cancellationToken);
        await conversationRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(conversation);
    }

    private static ConversationDto MapToDto(Conversation c)
    {
        var last = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
        var lastDto = last is null
            ? null
            : new MessageSummaryDto(last.SenderId, last.Content, last.SentAt, last.IsRead);
        return new ConversationDto(c.Id, c.User1Id, c.User2Id, c.CreatedAt, lastDto);
    }
}

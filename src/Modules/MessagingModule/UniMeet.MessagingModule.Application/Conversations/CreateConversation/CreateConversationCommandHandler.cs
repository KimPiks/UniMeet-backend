using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.CreateConversation;

public class CreateConversationCommandHandler(IConversationRepository conversationRepository)
    : ICommandHandler<CreateConversationCommand, ConversationDto>
{
    public async Task<ConversationDto> HandleAsync(CreateConversationCommand request, CancellationToken cancellationToken = default)
    {
        var existing = await conversationRepository.GetByUsersAsync(request.UserAId, request.UserBId, cancellationToken);
        if (existing is not null)
            return MapToDto(existing);

        var conversation = Conversation.Create(request.UserAId, request.UserBId);
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

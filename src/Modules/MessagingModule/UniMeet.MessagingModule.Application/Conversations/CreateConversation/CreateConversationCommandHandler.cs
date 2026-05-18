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
            return existing.ToDto(request.UserAId);

        var conversation = Conversation.Create(request.UserAId, request.UserBId);
        await conversationRepository.AddAsync(conversation, cancellationToken);
        await conversationRepository.SaveChangesAsync(cancellationToken);

        return conversation.ToDto(request.UserAId);
    }
}

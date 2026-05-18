using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Exceptions;

namespace UniMeet.MessagingModule.Application.Conversations.CreateGroupConversation;

public class CreateGroupConversationCommandHandler(IConversationRepository conversationRepository)
    : ICommandHandler<CreateGroupConversationCommand, ConversationDto>
{
    public async Task<ConversationDto> HandleAsync(CreateGroupConversationCommand request, CancellationToken cancellationToken = default)
    {
        var participantIds = request.ParticipantIds.ToArray();
        var errors = new List<string>();

        if (participantIds.Length < 2)
            errors.Add("A group conversation requires at least two invited participants.");

        if (participantIds.Contains(request.CreatorUserId))
            errors.Add("Creator cannot be included in participantIds.");

        if (participantIds.Distinct().Count() != participantIds.Length)
            errors.Add("participantIds cannot contain duplicates.");

        if (errors.Count > 0)
            throw new ValidationException(errors);

        var conversation = Conversation.CreateGroup(request.CreatorUserId, participantIds);
        await conversationRepository.AddAsync(conversation, cancellationToken);
        await conversationRepository.SaveChangesAsync(cancellationToken);

        return conversation.ToDto(request.CreatorUserId);
    }
}

using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Exceptions;

namespace UniMeet.MessagingModule.Application.Messages.SendMessage;

public class SendMessageCommandHandler(
    IConversationRepository conversationRepository,
    IMessageRepository messageRepository,
    IMessagingHubNotifier hubNotifier)
    : ICommandHandler<SendMessageCommand, MessageDto>
{
    public async Task<MessageDto> HandleAsync(SendMessageCommand request, CancellationToken cancellationToken = default)
    {
        var conversation = await conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new KeyNotFoundException($"Conversation {request.ConversationId} not found.");

        if (conversation.Participants.All(p => p.UserId != request.SenderId))
            throw new ForbiddenException("Sender is not a participant of this conversation.");

        var message = Message.Create(conversation.Id, request.SenderId, request.Content);
        await messageRepository.AddAsync(message, cancellationToken);
        await messageRepository.SaveChangesAsync(cancellationToken);

        var senderDto = message.ToDto(request.SenderId);

        var recipientIds = conversation.Participants
            .Select(p => p.UserId)
            .Where(id => id != request.SenderId)
            .ToArray();
        var recipientDto = new MessageDto(message.Id, message.ConversationId, message.SenderId, message.Content, message.SentAt, false);
        await hubNotifier.SendMessageAsync(recipientIds, recipientDto, cancellationToken);

        return senderDto;
    }
}

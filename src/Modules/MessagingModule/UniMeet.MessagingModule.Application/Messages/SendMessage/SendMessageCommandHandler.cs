using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.Shared.Abstractions;

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
            ?? throw new InvalidOperationException($"Conversation {request.ConversationId} not found.");

        if (conversation.User1Id != request.SenderId && conversation.User2Id != request.SenderId)
            throw new InvalidOperationException("Sender is not a participant of this conversation.");

        var message = Message.Create(conversation.Id, request.SenderId, request.Content);
        await messageRepository.AddAsync(message, cancellationToken);
        await messageRepository.SaveChangesAsync(cancellationToken);

        var dto = new MessageDto(message.Id, message.ConversationId, message.SenderId, message.Content, message.SentAt, message.IsRead);

        var recipientId = conversation.User1Id == request.SenderId ? conversation.User2Id : conversation.User1Id;
        await hubNotifier.SendMessageAsync(recipientId, dto, cancellationToken);

        return dto;
    }
}

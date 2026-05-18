using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Exceptions;

namespace UniMeet.MessagingModule.Application.Messages.MarkMessagesAsRead;

public class MarkMessagesAsReadCommandHandler(
    IConversationRepository conversationRepository,
    IMessageRepository messageRepository)
    : ICommandHandler<MarkMessagesAsReadCommand>
{
    public async Task HandleAsync(MarkMessagesAsReadCommand request, CancellationToken cancellationToken = default)
    {
        var isParticipant = await conversationRepository.IsParticipantAsync(request.ConversationId, request.RecipientId, cancellationToken);
        if (!isParticipant)
            throw new ForbiddenException("You are not a participant of this conversation.");

        var unreadMessageIds = await messageRepository.GetUnreadMessageIdsByConversationAndRecipientAsync(
            request.ConversationId, request.RecipientId, cancellationToken);

        var readReceipts = unreadMessageIds.Select(id => MessageReadReceipt.Create(id, request.RecipientId));
        await messageRepository.AddReadReceiptsAsync(readReceipts, cancellationToken);

        await messageRepository.SaveChangesAsync(cancellationToken);
    }
}

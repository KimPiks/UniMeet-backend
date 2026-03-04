using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Messages.MarkMessagesAsRead;

public class MarkMessagesAsReadCommandHandler(IMessageRepository messageRepository)
    : ICommandHandler<MarkMessagesAsReadCommand>
{
    public async Task HandleAsync(MarkMessagesAsReadCommand request, CancellationToken cancellationToken = default)
    {
        var unread = await messageRepository.GetUnreadByConversationAndRecipientAsync(
            request.ConversationId, request.RecipientId, cancellationToken);

        foreach (var message in unread)
            message.MarkAsRead();

        await messageRepository.SaveChangesAsync(cancellationToken);
    }
}

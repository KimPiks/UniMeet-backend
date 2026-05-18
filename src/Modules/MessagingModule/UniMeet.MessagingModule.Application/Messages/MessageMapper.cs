using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.MessagingModule.Application.Messages;

internal static class MessageMapper
{
    public static MessageDto ToDto(this Message message, Guid currentUserId)
    {
        var isRead = message.SenderId == currentUserId || message.ReadReceipts.Any(r => r.UserId == currentUserId);
        return new MessageDto(message.Id, message.ConversationId, message.SenderId, message.Content, message.SentAt, isRead);
    }
}

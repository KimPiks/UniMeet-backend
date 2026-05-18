using UniMeet.MessagingModule.Domain.Conversations;

namespace UniMeet.MessagingModule.Application.Conversations;

internal static class ConversationMapper
{
    public static ConversationDto ToDto(this Conversation conversation, Guid currentUserId)
    {
        var last = conversation.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault();
        var lastDto = last is null
            ? null
            : new MessageSummaryDto(
                last.SenderId,
                last.Content,
                last.SentAt,
                last.SenderId == currentUserId || last.ReadReceipts.Any(r => r.UserId == currentUserId));

        return new ConversationDto(
            conversation.Id,
            conversation.IsGroup,
            conversation.User1Id,
            conversation.User2Id,
            conversation.Participants.Select(p => p.UserId).OrderBy(id => id).ToArray(),
            conversation.CreatedAt,
            lastDto);
    }
}

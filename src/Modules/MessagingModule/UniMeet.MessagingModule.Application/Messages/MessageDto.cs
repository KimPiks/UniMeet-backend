namespace UniMeet.MessagingModule.Application.Messages;

public record MessageDto(
    Guid Id,
    Guid ConversationId,
    Guid SenderId,
    string Content,
    DateTime SentAt,
    bool IsRead);

namespace UniMeet.MessagingModule.Application.Conversations;

public record ConversationDto(
    Guid Id,
    Guid User1Id,
    Guid User2Id,
    DateTime CreatedAt,
    MessageSummaryDto? LastMessage);

public record MessageSummaryDto(
    Guid SenderId,
    string Content,
    DateTime SentAt,
    bool IsRead);

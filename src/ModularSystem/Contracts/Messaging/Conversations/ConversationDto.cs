namespace ModularSystem.Contracts.Messaging.Conversations;

public record ConversationDto(
    Guid Id,
    bool IsGroup,
    Guid? User1Id,
    Guid? User2Id,
    IReadOnlyCollection<Guid> ParticipantIds,
    DateTime CreatedAt,
    MessageSummaryDto? LastMessage);

public record MessageSummaryDto(
    Guid SenderId,
    string Content,
    DateTime SentAt,
    bool IsRead);

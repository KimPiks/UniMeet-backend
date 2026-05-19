namespace ModularSystem.Contracts.Messaging.Messages;

public record MessageDto(
    Guid Id,
    Guid ConversationId,
    Guid SenderId,
    string Content,
    DateTime SentAt,
    bool IsRead);

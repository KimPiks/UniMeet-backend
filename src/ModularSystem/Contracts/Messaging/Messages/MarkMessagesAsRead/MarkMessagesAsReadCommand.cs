using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Messaging.Messages.MarkMessagesAsRead;

public record MarkMessagesAsReadCommand(Guid ConversationId, Guid RecipientId) : ICommand;

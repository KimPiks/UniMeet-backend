using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Messages.MarkMessagesAsRead;

public record MarkMessagesAsReadCommand(Guid ConversationId, Guid RecipientId) : ICommand;

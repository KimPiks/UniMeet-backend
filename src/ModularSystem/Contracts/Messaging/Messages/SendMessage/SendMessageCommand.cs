using ModularSystem.Contracts.Messaging.Messages;
using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Messaging.Messages.SendMessage;

public record SendMessageCommand(Guid SenderId, Guid ConversationId, string Content) : ICommand<MessageDto>;

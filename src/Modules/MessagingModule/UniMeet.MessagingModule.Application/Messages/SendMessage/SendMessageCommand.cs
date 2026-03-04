using UniMeet.MessagingModule.Application.Messages;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Messages.SendMessage;

public record SendMessageCommand(Guid SenderId, Guid ConversationId, string Content) : ICommand<MessageDto>;

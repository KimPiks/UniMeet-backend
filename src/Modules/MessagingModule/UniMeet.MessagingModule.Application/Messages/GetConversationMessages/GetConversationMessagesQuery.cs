using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Messages.GetConversationMessages;

public record GetConversationMessagesQuery(Guid ConversationId, Guid CurrentUserId, int Offset = 0, int Limit = 50) : IQuery<IEnumerable<MessageDto>>;

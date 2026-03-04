using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.GetUserConversations;

public record GetUserConversationsQuery(Guid UserId) : IQuery<IEnumerable<ConversationDto>>;

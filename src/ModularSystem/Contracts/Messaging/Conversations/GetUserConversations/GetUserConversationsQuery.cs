using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Messaging.Conversations.GetUserConversations;

public record GetUserConversationsQuery(Guid UserId) : IQuery<IEnumerable<ConversationDto>>;

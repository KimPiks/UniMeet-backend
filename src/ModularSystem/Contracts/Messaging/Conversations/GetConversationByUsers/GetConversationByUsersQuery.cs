using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Messaging.Conversations.GetConversationByUsers;

public record GetConversationByUsersQuery(Guid UserAId, Guid UserBId) : IQuery<ConversationDto?>;

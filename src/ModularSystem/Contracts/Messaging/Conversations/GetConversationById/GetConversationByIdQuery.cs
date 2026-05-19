using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Messaging.Conversations.GetConversationById;

public record GetConversationByIdQuery(Guid ConversationId, Guid CurrentUserId) : IQuery<ConversationDto?>;

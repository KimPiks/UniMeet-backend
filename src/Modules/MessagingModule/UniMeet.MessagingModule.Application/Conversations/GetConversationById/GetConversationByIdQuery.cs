using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.GetConversationById;

public record GetConversationByIdQuery(Guid ConversationId) : IQuery<ConversationDto?>;

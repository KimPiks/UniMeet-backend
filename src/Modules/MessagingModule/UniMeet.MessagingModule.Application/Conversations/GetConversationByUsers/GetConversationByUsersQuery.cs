using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.GetConversationByUsers;

public record GetConversationByUsersQuery(Guid UserAId, Guid UserBId) : IQuery<ConversationDto?>;

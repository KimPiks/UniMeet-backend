using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.CreateConversation;

public record CreateConversationCommand(Guid UserAId, Guid UserBId) : ICommand<ConversationDto>;

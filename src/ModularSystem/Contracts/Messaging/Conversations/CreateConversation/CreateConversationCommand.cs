using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Messaging.Conversations.CreateConversation;

public record CreateConversationCommand(Guid UserAId, Guid UserBId) : ICommand<ConversationDto>;

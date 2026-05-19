using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Messaging.Conversations.CreateGroupConversation;

public record CreateGroupConversationCommand(Guid CreatorUserId, IReadOnlyCollection<Guid> ParticipantIds) : ICommand<ConversationDto>;

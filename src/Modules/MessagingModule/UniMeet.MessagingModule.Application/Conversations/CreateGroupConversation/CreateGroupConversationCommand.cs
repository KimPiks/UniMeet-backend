using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.CreateGroupConversation;

public record CreateGroupConversationCommand(Guid CreatorUserId, IReadOnlyCollection<Guid> ParticipantIds) : ICommand<ConversationDto>;

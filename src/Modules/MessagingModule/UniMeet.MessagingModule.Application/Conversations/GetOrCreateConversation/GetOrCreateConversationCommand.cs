using UniMeet.MessagingModule.Application.Conversations;
using UniMeet.MessagingModule.Domain.Messages;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MessagingModule.Application.Conversations.GetOrCreateConversation;

public record GetOrCreateConversationCommand(Guid RequestingUserId, Guid OtherUserId) : ICommand<ConversationDto>;

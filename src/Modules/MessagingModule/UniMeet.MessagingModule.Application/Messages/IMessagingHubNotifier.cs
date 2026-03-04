namespace UniMeet.MessagingModule.Application.Messages;

public interface IMessagingHubNotifier
{
    Task SendMessageAsync(Guid recipientUserId, MessageDto message, CancellationToken cancellationToken = default);
}

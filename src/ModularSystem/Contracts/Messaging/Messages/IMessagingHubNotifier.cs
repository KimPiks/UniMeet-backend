namespace ModularSystem.Contracts.Messaging.Messages;

public interface IMessagingHubNotifier
{
    Task SendMessageAsync(IEnumerable<Guid> recipientUserIds, MessageDto message, CancellationToken cancellationToken = default);
}

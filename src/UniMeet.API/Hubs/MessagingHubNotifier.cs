using Microsoft.AspNetCore.SignalR;
using ModularSystem.Contracts.Messaging.Messages;

namespace UniMeet.API.Hubs;

public class MessagingHubNotifier(IHubContext<MessagingHub> hubContext) : IMessagingHubNotifier
{
    public async Task SendMessageAsync(IEnumerable<Guid> recipientUserIds, MessageDto message, CancellationToken cancellationToken = default)
    {
        await hubContext.Clients
            .Groups(recipientUserIds.Select(id => id.ToString()))
            .SendAsync("ReceiveMessage", message, cancellationToken);
    }
}

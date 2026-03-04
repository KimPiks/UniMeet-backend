using Microsoft.AspNetCore.SignalR;
using UniMeet.MessagingModule.Application.Messages;

namespace UniMeet.API.Hubs;

public class MessagingHubNotifier(IHubContext<MessagingHub> hubContext) : IMessagingHubNotifier
{
    public async Task SendMessageAsync(Guid recipientUserId, MessageDto message, CancellationToken cancellationToken = default)
    {
        await hubContext.Clients
            .Group(recipientUserId.ToString())
            .SendAsync("ReceiveMessage", message, cancellationToken);
    }
}

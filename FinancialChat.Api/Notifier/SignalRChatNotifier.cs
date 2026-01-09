using FinancialChat.Application.Interfaces;
using FinancialChat.Api.Hubs;
using FinancialChat.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Api.Notifiers;

public class SignalRChatNotifier : IChatNotifier {
    private readonly IHubContext<ChatHub> _hubContext;

    public SignalRChatNotifier(IHubContext<ChatHub> hubContext) {
        _hubContext = hubContext;
    }

    public async Task NotifyMessageAsync(ChatMessage message, CancellationToken cancellationToken) 
    {
        await _hubContext.Clients.All.SendAsync(
            "ReceiveMessage",
            message.UserName,
            message.Content,
            cancellationToken);
    }
}

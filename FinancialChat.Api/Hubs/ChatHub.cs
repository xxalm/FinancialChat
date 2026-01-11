using FinancialChat.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Api.Hubs;

[Authorize]
public class ChatHub : Hub {
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService) 
    {
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync() 
    {
        var chatRoomId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var messages = await _chatService.GetLastMessagesAsync(
            chatRoomId,
            Context.ConnectionAborted);

        foreach (var message in messages) 
        {
            await Clients.Caller.SendAsync(
                "ReceiveMessage",
                message.UserId,
                message.Content);
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(Guid chatRoomId, string message) 
    {
        var userId = Context.UserIdentifier;

        if (string.IsNullOrEmpty(userId))
            throw new HubException("User not authenticated");

        await _chatService.SendMessageAsync(
            chatRoomId,
            userId,
            message,
            Context.ConnectionAborted);
    }
}

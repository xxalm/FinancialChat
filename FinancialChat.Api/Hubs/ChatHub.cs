using FinancialChat.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChat.Api.Hubs;

public class ChatHub : Hub {
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService) {
        _chatService = chatService;
    }

    public async Task SendMessage(Guid chatRoomId, string user, string message) 
    {
        await _chatService.SendMessageAsync(
            chatRoomId,
            user,
            message
        );
    }
}

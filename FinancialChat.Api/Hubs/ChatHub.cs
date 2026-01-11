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

    public override async Task OnConnectedAsync() {
        Console.WriteLine(">>> ChatHub connected");

        var userId = Context.UserIdentifier;
        Console.WriteLine($">>> UserIdentifier: {userId}");

        if (string.IsNullOrEmpty(userId))
            throw new HubException("UserIdentifier is null");

        var chatRoomId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var messages = await _chatService.GetLastMessagesAsync(
            chatRoomId,
            Context.ConnectionAborted);

        foreach (var message in messages) {
            await Clients.Caller.SendAsync(
                "ReceiveMessage",
                new {
                    userId = message.UserId,
                    content = message.Content,
                    isFromBot = message.IsFromBot
                });
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(Guid chatRoomId, string message) {
        try {
            var userId = Context.UserIdentifier;

            Console.WriteLine(">>> SendMessage called");
            Console.WriteLine("UserIdentifier: " + userId);
            Console.WriteLine("ChatRoomId: " + chatRoomId);
            Console.WriteLine("Message: " + message);

            if (string.IsNullOrEmpty(userId))
                throw new Exception("UserIdentifier is null");

            await _chatService.SendMessageAsync(
                chatRoomId,
                userId,
                message,
                Context.ConnectionAborted);

            Console.WriteLine(">>> SendMessage completed");
        } catch (Exception ex) {
            Console.WriteLine(">>> SendMessage ERROR");
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
}

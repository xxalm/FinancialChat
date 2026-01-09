using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Domain.Entities;

public class ChatMessage {
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ChatRoomId { get; private set; }
    public string UserName { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public bool IsFromBot { get; private set; }

    protected ChatMessage() { }

    public ChatMessage(
        Guid chatRoomId,
        string userName,
        string content,
        bool isFromBot = false) {
        ChatRoomId = chatRoomId;
        UserName = userName;
        Content = content;
        IsFromBot = isFromBot;
        CreatedAt = DateTime.UtcNow;
    }
}



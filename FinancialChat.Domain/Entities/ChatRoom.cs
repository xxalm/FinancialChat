using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Domain.Entities;

public class ChatRoom {
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public ICollection<ChatMessage> Messages { get; private set; } = new List<ChatMessage>();

    protected ChatRoom() { }

    public ChatRoom(Guid id, string name) {
        Id = id;
        Name = name;
    }

    public ChatRoom(string name) {
        Id = Guid.NewGuid();
        Name = name;
    }
}

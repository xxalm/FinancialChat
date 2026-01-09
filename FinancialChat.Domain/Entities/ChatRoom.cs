using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Domain.Entities;

public class ChatRoom {
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = null!;

    protected ChatRoom() { }

    public ChatRoom(string name) {
        Name = name;
    }
}

using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.Interfaces;

public interface IChatService 
{
    Task<IReadOnlyList<ChatMessage>> GetLastMessagesAsync(Guid chatRoomId, CancellationToken cancellationToken = default);
    Task SendMessageAsync(Guid chatRoomId, string userId, string content, CancellationToken cancellationToken = default);
}

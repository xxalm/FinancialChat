using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.Interfaces;

public interface IChatMessageRepository {
    Task AddAsync(ChatMessage message, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ChatMessage>> GetLastMessagesAsync(
        Guid chatRoomId,
        int limit = 50,
        CancellationToken cancellationToken = default);
}

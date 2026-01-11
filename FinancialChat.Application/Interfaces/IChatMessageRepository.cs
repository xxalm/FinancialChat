using FinancialChat.Domain.Entities;

public interface IChatMessageRepository 
{
    Task<IReadOnlyList<ChatMessage>> GetLastMessagesAsync(
        Guid chatRoomId,
        int limit,
        CancellationToken cancellationToken);

    Task AddAsync(ChatMessage message, CancellationToken cancellationToken);
}

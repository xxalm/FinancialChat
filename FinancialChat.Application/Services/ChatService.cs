using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.Services;

public class ChatService : IChatService {
    private readonly IChatMessageRepository _repository;
    private readonly IChatNotifier _notifier;
    private readonly IStockCommandPublisher _stockPublisher;

    public ChatService(IChatMessageRepository repository, IChatNotifier notifier, IStockCommandPublisher stockPublisher) 
    {
        _repository = repository;
        _notifier = notifier;
        _stockPublisher = stockPublisher;
    }

    public async Task SendMessageAsync(Guid chatRoomId, string userId, string content, CancellationToken cancellationToken = default) 
    {
        
        if (content.StartsWith("/stock=", StringComparison.OrdinalIgnoreCase)) 
        {
            var stockCode = content
                .Replace("/stock=", "", StringComparison.OrdinalIgnoreCase)
                .Trim();

            if (!string.IsNullOrWhiteSpace(stockCode)) {
                await _stockPublisher.PublishAsync(
                    chatRoomId,
                    stockCode,
                    cancellationToken);
            }

            return;
        }

        var message = new ChatMessage(
            chatRoomId,
            userId,
            content,
            isFromBot: false);

        await _repository.AddAsync(message, cancellationToken);
        await _notifier.NotifyMessageAsync(message, cancellationToken);
    }

    public async Task<IReadOnlyList<ChatMessage>> GetLastMessagesAsync(Guid chatRoomId, CancellationToken cancellationToken = default) 
    {
        return await _repository.GetLastMessagesAsync(chatRoomId, limit: 50, cancellationToken);
    }
}

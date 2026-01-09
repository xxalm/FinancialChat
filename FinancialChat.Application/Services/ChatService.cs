using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.Services;

public class ChatService : IChatService {
    private readonly IChatMessageRepository _repository;
    private readonly IChatNotifier _notifier;

    public ChatService(
        IChatMessageRepository repository,
        IChatNotifier notifier) {
        _repository = repository;
        _notifier = notifier;
    }

    public async Task SendMessageAsync(
        Guid chatRoomId,
        string userName,
        string content,
        bool isFromBot = false,
        CancellationToken cancellationToken = default) {
        var message = new ChatMessage(
            chatRoomId,
            userName,
            content,
            isFromBot);

        await _repository.AddAsync(message, cancellationToken);
        await _notifier.NotifyMessageAsync(message, cancellationToken);
    }
}

using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.Services;

public class StockBotService : IStockCommandConsumer 
{
    private readonly IChatNotifier _notifier;

    public StockBotService(IChatNotifier notifier) 
    {
        _notifier = notifier;
    }

    public async Task HandleAsync(Guid chatRoomId, string stockCode, CancellationToken cancellationToken = default) 
    {
        var content = $"{stockCode.ToUpper()} quote is $0.00 per share";

        var message = new ChatMessage(
            chatRoomId,
            "bot",
            content,
            isFromBot: true);

        await _notifier.NotifyMessageAsync(message, cancellationToken);
    }
}

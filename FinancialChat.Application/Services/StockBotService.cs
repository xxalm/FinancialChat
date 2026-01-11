using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Entities;
using System.Globalization;

namespace FinancialChat.Application.Services;

public class StockBotService : IStockBotService {
    private readonly IChatNotifier _notifier;
    private readonly IStockQuoteClient _stockClient;


    public StockBotService(IChatNotifier notifier, IStockQuoteClient stockClient) 
    {
        _notifier = notifier;
        _stockClient = stockClient;
    }

    public async Task HandleAsync(Guid chatRoomId, string stockCode, CancellationToken cancellationToken = default) 
    {
        var quote = await _stockClient.GetQuoteAsync(stockCode, cancellationToken);

        var content = quote.HasValue
            ? $"{stockCode.ToUpper()} quote is ${quote.Value.ToString(CultureInfo.InvariantCulture)} per share"
            : $"Could not retrieve quote for {stockCode.ToUpper()}";


        var message = new ChatMessage(
            chatRoomId,
            "bot",
            content,
            isFromBot: true);

        await _notifier.NotifyMessageAsync(message, cancellationToken);
    }
}

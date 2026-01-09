using FinancialChat.Application.Interfaces;

namespace FinancialChat.Infrastructure.Messaging;

public class InMemoryStockCommandPublisher : IStockCommandPublisher 
{
    private readonly IStockCommandConsumer _consumer;

    public InMemoryStockCommandPublisher(IStockCommandConsumer consumer) 
    {
        _consumer = consumer;
    }

    public async Task PublishAsync(Guid chatRoomId, string stockCode, CancellationToken cancellationToken = default) 
    {
        await _consumer.HandleAsync(chatRoomId, stockCode, cancellationToken);
    }
}

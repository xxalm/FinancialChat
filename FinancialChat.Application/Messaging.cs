using FinancialChat.Application.Interfaces;

namespace FinancialChat.Infrastructure.Messaging;

public class InMemoryStockCommandPublisher : IStockCommandPublisher {
    public Task PublishAsync(
        Guid chatRoomId,
        string stockCode,
        CancellationToken cancellationToken = default) {
        // Placeholder – RabbitMQ vem depois
        return Task.CompletedTask;
    }
}

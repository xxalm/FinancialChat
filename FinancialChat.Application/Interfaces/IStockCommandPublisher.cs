namespace FinancialChat.Application.Interfaces;

public interface IStockCommandPublisher {
    Task PublishAsync(
        Guid chatRoomId,
        string stockCode,
        CancellationToken cancellationToken = default);
}

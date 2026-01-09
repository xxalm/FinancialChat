using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.Interfaces;

public interface IStockCommandConsumer {
    Task HandleAsync(
        Guid chatRoomId,
        string stockCode,
        CancellationToken cancellationToken = default);
}

namespace FinancialChat.Application.Interfaces;

public interface IStockQuoteClient {
    Task<decimal?> GetQuoteAsync(
        string stockCode,
        CancellationToken cancellationToken = default);
}

namespace FinancialChat.Infrastructure.Messaging.Contracts;

public sealed class StockCommandMessage {
    public Guid ChatRoomId { get; init; }
    public string StockCode { get; init; } = string.Empty;
}

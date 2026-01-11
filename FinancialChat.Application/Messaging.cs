namespace FinancialChat.Application.Messaging;

public record StockCommandMessage(Guid ChatRoomId, string StockCode);

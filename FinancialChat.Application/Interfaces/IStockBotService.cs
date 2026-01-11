public interface IStockBotService {
    Task HandleAsync(Guid chatRoomId, string stockCode, CancellationToken cancellationToken = default);
}

using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.Interfaces;

public interface IChatService {
    Task SendMessageAsync(
        Guid chatRoomId,
        string userName,
        string content,
        bool isFromBot = false,
        CancellationToken cancellationToken = default);
}

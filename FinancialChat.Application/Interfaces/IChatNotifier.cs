using FinancialChat.Domain.Entities;

namespace FinancialChat.Application.Interfaces;

public interface IChatNotifier {
    Task NotifyMessageAsync(
        ChatMessage message,
        CancellationToken cancellationToken = default);
}

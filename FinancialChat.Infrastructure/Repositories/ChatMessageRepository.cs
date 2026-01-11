using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Entities;
using FinancialChat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Infrastructure.Repositories;

public class ChatMessageRepository : IChatMessageRepository {
    private readonly ChatDbContext _context;

    public ChatMessageRepository(ChatDbContext context) {
        _context = context;
    }

    public async Task AddAsync(
        ChatMessage message,
        CancellationToken cancellationToken = default) {
        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ChatMessage>> GetLastMessagesAsync(Guid chatRoomId, int limit, CancellationToken cancellationToken) 
    {
        return await _context.ChatMessages
            .Where(x => x.ChatRoomId == chatRoomId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(limit)
            .OrderBy(x => x.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

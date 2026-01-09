using FinancialChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Infrastructure.Persistence;

public class ChatDbContext : DbContext {
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options) { }

    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ChatMessage>(entity => {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<ChatRoom>(entity => {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        });
    }
}

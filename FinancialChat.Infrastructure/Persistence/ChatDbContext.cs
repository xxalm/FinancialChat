using FinancialChat.Domain.Entities;
using FinancialChat.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialChat.Infrastructure.Persistence;

public class ChatDbContext
    : IdentityDbContext<ApplicationUser> {
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options) { }

    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChatMessage>(entity => {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.ChatRoom)
                .WithMany()
                .HasForeignKey(x => x.ChatRoomId);

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

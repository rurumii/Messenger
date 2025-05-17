using Microsoft.EntityFrameworkCore;
using MessageService.Models;
namespace MessageService.Data
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Chat: один уникальный чат между двумя пользователями
            modelBuilder.Entity<Chat>()
                .HasIndex(c => new { c.User1Id, c.User2Id })
                .IsUnique();

            // Message → Chat (один чат содержит много сообщений)
            modelBuilder.Entity<Message>()
                .HasOne<Chat>()
                .WithMany()
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}

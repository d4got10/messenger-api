using Microsoft.EntityFrameworkCore;

namespace messanger.Models
{
    public class MessagesContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public MessagesContext(DbContextOptions<MessagesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasKey(message => new { message.SenderId, message.ReceiverId, message.Date });
        }
    }
}

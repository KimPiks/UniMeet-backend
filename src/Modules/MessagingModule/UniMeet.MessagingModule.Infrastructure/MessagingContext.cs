using Microsoft.EntityFrameworkCore;
using UniMeet.MessagingModule.Domain.Conversations;
using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.MessagingModule.Infrastructure;

public class MessagingContext : DbContext
{
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }

    public MessagingContext(DbContextOptions<MessagingContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessagingContext).Assembly);
    }
}

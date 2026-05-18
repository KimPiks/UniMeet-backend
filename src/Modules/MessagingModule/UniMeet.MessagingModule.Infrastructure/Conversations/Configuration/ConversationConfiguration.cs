using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.MessagingModule.Domain.Conversations;

namespace UniMeet.MessagingModule.Infrastructure.Conversations.Configuration;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("conversations");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.IsGroup).IsRequired();
        builder.Property(c => c.CreatedByUserId);
        builder.Property(c => c.User1Id);
        builder.Property(c => c.User2Id);
        builder.Property(c => c.CreatedAt).IsRequired();

        // Enforce uniqueness only for private conversations.
        builder.HasIndex(c => new { c.User1Id, c.User2Id })
            .IsUnique()
            .HasFilter("\"IsGroup\" = FALSE AND \"User1Id\" IS NOT NULL AND \"User2Id\" IS NOT NULL");

        builder.HasMany(c => c.Messages)
            .WithOne()
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Participants)
            .WithOne()
            .HasForeignKey(p => p.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

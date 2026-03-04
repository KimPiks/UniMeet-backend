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

        builder.Property(c => c.User1Id).IsRequired();
        builder.Property(c => c.User2Id).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();

        // Enforce uniqueness of the user pair (User1Id is always the smaller GUID)
        builder.HasIndex(c => new { c.User1Id, c.User2Id }).IsUnique();

        builder.HasMany(c => c.Messages)
            .WithOne()
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

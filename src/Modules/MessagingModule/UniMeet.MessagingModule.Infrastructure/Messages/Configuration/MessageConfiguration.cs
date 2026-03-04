using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.MessagingModule.Infrastructure.Messages.Configuration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.ConversationId).IsRequired();
        builder.Property(m => m.SenderId).IsRequired();
        builder.Property(m => m.Content).IsRequired().HasMaxLength(4000);
        builder.Property(m => m.SentAt).IsRequired();
        builder.Property(m => m.IsRead).IsRequired();

        builder.HasIndex(m => m.ConversationId);
        builder.HasIndex(m => new { m.ConversationId, m.SentAt });
    }
}

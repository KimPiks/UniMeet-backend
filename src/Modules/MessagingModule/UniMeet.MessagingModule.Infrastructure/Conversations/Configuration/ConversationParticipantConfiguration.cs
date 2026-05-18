using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.MessagingModule.Domain.Conversations;

namespace UniMeet.MessagingModule.Infrastructure.Conversations.Configuration;

public class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipant>
{
    public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
    {
        builder.ToTable("conversation_participants");

        builder.HasKey(p => new { p.ConversationId, p.UserId });

        builder.Property(p => p.ConversationId).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.JoinedAt).IsRequired();

        builder.HasIndex(p => p.UserId);
    }
}

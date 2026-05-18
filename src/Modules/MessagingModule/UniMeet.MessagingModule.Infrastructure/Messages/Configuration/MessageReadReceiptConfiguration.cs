using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.MessagingModule.Domain.Messages;

namespace UniMeet.MessagingModule.Infrastructure.Messages.Configuration;

public class MessageReadReceiptConfiguration : IEntityTypeConfiguration<MessageReadReceipt>
{
    public void Configure(EntityTypeBuilder<MessageReadReceipt> builder)
    {
        builder.ToTable("message_read_receipts");

        builder.HasKey(r => new { r.MessageId, r.UserId });

        builder.Property(r => r.MessageId).IsRequired();
        builder.Property(r => r.UserId).IsRequired();
        builder.Property(r => r.ReadAt).IsRequired();

        builder.HasIndex(r => r.UserId);
    }
}

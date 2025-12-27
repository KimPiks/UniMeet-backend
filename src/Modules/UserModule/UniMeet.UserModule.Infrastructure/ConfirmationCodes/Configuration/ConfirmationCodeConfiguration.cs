using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UserModule.Domain.ConfirmationCodes;

namespace UniMeet.UserModule.Infrastructure.ConfirmationCodes.Configuration;

public class ConfirmationCodeConfiguration : IEntityTypeConfiguration<ConfirmationCode>
{
    public void Configure(EntityTypeBuilder<ConfirmationCode> builder)
    {
        builder.ToTable("confirmation_codes");

        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Code)
            .IsRequired();
        
        builder.Property(c => c.UserId)
            .IsRequired();
        
        builder.HasOne(c => c.User)
            .WithMany(u => u.ConfirmationCodes)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(c => c.Code)
            .IsUnique();
    }
}
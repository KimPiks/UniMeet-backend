using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UserModule.Domain.PasswordResetCodes;

namespace UniMeet.UserModule.Infrastructure.PasswordResetCodes.Configuration;

public class PasswordResetCodesConfiguration : IEntityTypeConfiguration<PasswordResetCode>
{
    public void Configure(EntityTypeBuilder<PasswordResetCode> builder)
    {
        builder.ToTable("password_reset_codes");

        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Code)
            .IsRequired();
        
        builder.Property(c => c.UserId)
            .IsRequired();
        
        builder.HasOne(c => c.User)
            .WithMany(u => u.PasswordResetCodes)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(c => c.Code)
            .IsUnique();
    }
}
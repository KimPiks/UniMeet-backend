using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UserModule.Domain.RefreshTokens;

namespace UniMeet.UserModule.Infrastructure.RefreshTokens.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Token)
            .IsRequired();
        
        builder.Property(c => c.UserId)
            .IsRequired();
        
        builder.HasOne(c => c.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(c => c.Token)
            .IsUnique();
    }
}
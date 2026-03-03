using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UserModule.Domain.UserDetails;

namespace UniMeet.UserModule.Infrastructure.UserDetails.Configuration;

public class UserDetailConfiguration : IEntityTypeConfiguration<UserDetail>
{
    public void Configure(EntityTypeBuilder<UserDetail> builder)
    {
        builder.ToTable("user_details");

        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Sex)
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(c => c.ProfilePicturePath)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(c => c.ProfilePictureMimeType)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.HasMany(c => c.Interests)
            .WithMany(); 
        
        builder.HasOne(c => c.User)
            .WithOne(u => u.UserDetail)
            .HasForeignKey<UserDetail>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

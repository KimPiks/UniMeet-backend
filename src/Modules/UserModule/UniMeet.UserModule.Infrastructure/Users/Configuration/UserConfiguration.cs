using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Infrastructure.Users.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(64);
        
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();
        
        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired();
        
        builder.HasOne(u => u.UserDetail)
            .WithOne(ud => ud.User)
            .HasForeignKey<User>(u => u.UserDetailId);
    }
}

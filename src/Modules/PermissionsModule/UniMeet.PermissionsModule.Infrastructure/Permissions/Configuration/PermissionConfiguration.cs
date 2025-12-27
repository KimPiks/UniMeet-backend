using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PermissionsModule.Domain.Permissions;

namespace PermissionsModule.Infrastructure.Permissions.Configuration;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.PermissionName)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasOne(p => p.Group)
            .WithMany(p => p.Permissions)
            .HasForeignKey(p => p.GroupId);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Infrastructure.Universities.Configuration;

public class UniversityConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder.ToTable("universities");

        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.Property(u => u.Country)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.Property(u => u.Voivodeship)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.Property(u => u.City)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.Address)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasMany(u => u.Departments)
            .WithOne(d => d.University)
            .HasForeignKey(d => d.UniversityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.AllowedEmailDomains)
            .WithOne(a => a.University)
            .HasForeignKey(a => a.UniversityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
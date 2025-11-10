using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Infrastructure.Universities.Configuration;

public class AllowedEmailDomainConfiguration : IEntityTypeConfiguration<AllowedEmailDomain>
{
    public void Configure(EntityTypeBuilder<AllowedEmailDomain> builder)
    {
        builder.ToTable("allowed_email_domains");
        
        builder.HasKey(aed => aed.Id);
        
        builder.Property(aed => aed.Domain)
            .IsRequired()
            .HasMaxLength(128);
    }
}
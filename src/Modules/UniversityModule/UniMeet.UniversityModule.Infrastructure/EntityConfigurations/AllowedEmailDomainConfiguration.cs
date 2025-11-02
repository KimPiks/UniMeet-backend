using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Infrastructure.EntityConfigurations;

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
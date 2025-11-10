using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Infrastructure.Universities.Configuration;

public class FieldOfStudyConfiguration : IEntityTypeConfiguration<FieldOfStudy>
{
    public void Configure(EntityTypeBuilder<FieldOfStudy> builder)
    {
        builder.ToTable("fields_of_study");
        
        builder.HasKey(fos => fos.Id);

        builder.Property(fos => fos.Name)
            .IsRequired()
            .HasMaxLength(128);
    }
}
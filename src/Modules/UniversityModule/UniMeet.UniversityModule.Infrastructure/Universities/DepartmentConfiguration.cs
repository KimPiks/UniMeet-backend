using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Infrastructure.Universities;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.HasMany(d => d.FieldsOfStudy)
            .WithOne(fos => fos.Department)
            .HasForeignKey(fos => fos.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
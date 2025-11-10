using Microsoft.EntityFrameworkCore;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Infrastructure;

public class UniversityContext : DbContext
{
    public DbSet<University> Universities { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<FieldOfStudy> FieldsOfStudy { get; set; } = null!;
    public DbSet<AllowedEmailDomain> AllowedEmailDomains { get; set; } = null!;
    
    public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UniversityContext).Assembly);
    }
}
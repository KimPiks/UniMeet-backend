using Microsoft.EntityFrameworkCore;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UserEnrollmentModule.Infrastructure;

public class UserEnrollmentContext : DbContext
{
    public DbSet<Domain.UserAffiliation.UserAffiliation> UserAffiliations { get; set; } = null!;
    
    public UserEnrollmentContext(DbContextOptions<UserEnrollmentContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEnrollmentContext).Assembly);
    }
}
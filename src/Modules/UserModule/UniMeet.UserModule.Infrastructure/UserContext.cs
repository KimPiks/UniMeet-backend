using Microsoft.EntityFrameworkCore;
using UniMeet.UserModule.Domain.ConfirmationCodes;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Infrastructure;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ConfirmationCode> ConfirmationCodes { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserContext).Assembly);
    }
}
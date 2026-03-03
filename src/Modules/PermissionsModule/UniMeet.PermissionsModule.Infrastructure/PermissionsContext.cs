using Microsoft.EntityFrameworkCore;
using PermissionsModule.Domain.Groups;
using PermissionsModule.Domain.Permissions;

namespace PermissionsModule.Infrastructure;

public class PermissionsContext : DbContext
{
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Permission> Permissions => Set<Permission>();
    
    public PermissionsContext(DbContextOptions<PermissionsContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PermissionsContext).Assembly);
    }}

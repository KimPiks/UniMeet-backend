using Microsoft.EntityFrameworkCore;
using UniMeet.MatchingModule.Domain.Likes;
using UniMeet.MatchingModule.Domain.Matches;

namespace UniMeet.MatchingModule.Infrastructure;

public class MatchingContext : DbContext
{
    public DbSet<Like> Likes { get; set; }
    public DbSet<Match> Matches { get; set; }

    public MatchingContext(DbContextOptions<MatchingContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MatchingContext).Assembly);
    }
}

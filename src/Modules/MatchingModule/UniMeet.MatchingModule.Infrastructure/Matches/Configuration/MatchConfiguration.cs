using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.MatchingModule.Domain.Matches;

namespace UniMeet.MatchingModule.Infrastructure.Matches.Configuration;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("matches");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.User1Id).IsRequired();
        builder.Property(m => m.User2Id).IsRequired();
        builder.Property(m => m.CreatedAt).IsRequired();

        // Enforce uniqueness of the user pair (User1Id is always the smaller GUID).
        builder.HasIndex(m => new { m.User1Id, m.User2Id }).IsUnique();
    }
}

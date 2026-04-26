using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniMeet.MatchingModule.Domain.Likes;

namespace UniMeet.MatchingModule.Infrastructure.Likes.Configuration;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.ToTable("likes");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.LikerId).IsRequired();
        builder.Property(l => l.LikedId).IsRequired();
        builder.Property(l => l.CreatedAt).IsRequired();

        // Each (liker, liked) pair must be unique.
        builder.HasIndex(l => new { l.LikerId, l.LikedId }).IsUnique();
        builder.HasIndex(l => l.LikedId);
    }
}

namespace UniMeet.MatchingModule.Domain.Likes;

public class Like
{
    public Guid Id { get; set; }
    public Guid LikerId { get; set; }
    public Guid LikedId { get; set; }
    public DateTime CreatedAt { get; set; }

    private Like() { }

    public static Like Create(Guid likerId, Guid likedId)
    {
        if (likerId == likedId)
            throw new InvalidOperationException("A user cannot like themselves.");

        return new Like
        {
            Id = Guid.NewGuid(),
            LikerId = likerId,
            LikedId = likedId,
            CreatedAt = DateTime.UtcNow
        };
    }
}

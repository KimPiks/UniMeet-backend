namespace UniMeet.MatchingModule.Domain.Matches;

public class Match
{
    public Guid Id { get; set; }

    // User1Id is always the smaller GUID to enforce uniqueness regardless of order.
    public Guid User1Id { get; set; }
    public Guid User2Id { get; set; }

    public DateTime CreatedAt { get; set; }

    private Match() { }

    public static Match Create(Guid userA, Guid userB)
    {
        if (userA == userB)
            throw new InvalidOperationException("A user cannot match with themselves.");

        var (user1, user2) = userA.CompareTo(userB) < 0 ? (userA, userB) : (userB, userA);
        return new Match
        {
            Id = Guid.NewGuid(),
            User1Id = user1,
            User2Id = user2,
            CreatedAt = DateTime.UtcNow
        };
    }
}

using UniMeet.MatchingModule.Domain.Likes;
using UniMeet.MatchingModule.Domain.Matches;

namespace UniMeet.UnitTests.Modules;

public class MatchingDomainTests
{
    [Fact]
    public void Like_Create_rejects_self_like()
    {
        var userId = Guid.NewGuid();

        var exception = Assert.Throws<InvalidOperationException>(() => Like.Create(userId, userId));

        Assert.Equal("A user cannot like themselves.", exception.Message);
    }

    [Fact]
    public void Match_Create_orders_user_ids_independently_of_argument_order()
    {
        var higher = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        var lower = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var match = Match.Create(higher, lower);

        Assert.Equal(lower, match.User1Id);
        Assert.Equal(higher, match.User2Id);
        Assert.NotEqual(Guid.Empty, match.Id);
        Assert.True(match.CreatedAt <= DateTime.UtcNow);
    }
}

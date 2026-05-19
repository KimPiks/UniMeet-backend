using UniMeet.UserModule.Domain.Interests;
using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UnitTests.Modules;

public class UserDomainTests
{
    [Fact]
    public void Constructor_creates_inactive_user_with_detail()
    {
        var user = new User("Anna", "Kowalska", "anna@uni.edu", "hash", 10, 3, Sex.Female);

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.False(user.IsActive);
        Assert.Equal(10, user.UniversityId);
        Assert.Equal(3, user.GroupId);
        Assert.Equal(user.Id, user.UserDetail.UserId);
        Assert.Equal(Sex.Female, user.UserDetail.Sex);
    }

    [Fact]
    public void Activate_sets_active_flag_and_updates_timestamp()
    {
        var user = CreateUser();
        var previousUpdatedAt = user.UpdatedAt;

        user.Activate();

        Assert.True(user.IsActive);
        Assert.True(user.UpdatedAt >= previousUpdatedAt);
    }

    [Fact]
    public void UserDetail_AddInterest_does_not_add_same_instance_twice()
    {
        var user = CreateUser();
        var interest = new Interest { Id = 1, Name = "Chess" };

        user.UserDetail.AddInterest(interest);
        user.UserDetail.AddInterest(interest);

        Assert.Single(user.UserDetail.Interests);
        Assert.Same(interest, user.UserDetail.Interests.Single());
    }

    [Fact]
    public void UserDetail_profile_picture_can_be_set_and_removed()
    {
        var user = CreateUser();

        user.UserDetail.SetProfilePicture("avatars/user.png", "image/png");
        user.UserDetail.RemoveProfilePicture();

        Assert.Null(user.UserDetail.ProfilePicturePath);
        Assert.Null(user.UserDetail.ProfilePictureMimeType);
    }

    private static User CreateUser()
        => new("Anna", "Kowalska", "anna@uni.edu", "hash", 10, 3, Sex.Female);
}

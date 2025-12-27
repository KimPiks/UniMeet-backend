using FluentAssertions;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Domain.Tests.Users;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldCreateUserWithValidProperties()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var passwordHash = "hashed_password";
        var universityId = 1;
        var groupId = 2;

        // Act
        var user = new User(firstName, lastName, email, passwordHash, universityId, groupId);

        // Assert
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
        user.UniversityId.Should().Be(universityId);
        user.GroupId.Should().Be(groupId);
        user.IsActive.Should().BeFalse();
        user.Id.Should().NotBe(Guid.Empty);
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdatePassword_ShouldUpdatePasswordHashAndUpdatedAt()
    {
        // Arrange
        var user = new User("John", "Doe", "john@example.com", "old_hash", 1, 1);
        var oldUpdatedAt = user.UpdatedAt;
        var newPasswordHash = "new_hash";

        // Act
        user.UpdatePassword(newPasswordHash);

        // Assert
        user.PasswordHash.Should().Be(newPasswordHash);
        user.UpdatedAt.Should().BeOnOrAfter(oldUpdatedAt);
    }

    [Fact]
    public void Rename_ShouldUpdateFirstNameLastNameAndUpdatedAt()
    {
        // Arrange
        var user = new User("John", "Doe", "john@example.com", "hash", 1, 1);
        var oldUpdatedAt = user.UpdatedAt;
        var newFirstName = "Jane";
        var newLastName = "Smith";

        // Act
        user.Rename(newFirstName, newLastName);

        // Assert
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().Be(newLastName);
        user.UpdatedAt.Should().BeOnOrAfter(oldUpdatedAt);
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrueAndUpdateUpdatedAt()
    {
        // Arrange
        var user = new User("John", "Doe", "john@example.com", "hash", 1, 1);
        var oldUpdatedAt = user.UpdatedAt;

        // Act
        user.Activate();

        // Assert
        user.IsActive.Should().BeTrue();
        user.UpdatedAt.Should().BeOnOrAfter(oldUpdatedAt);
    }

    [Fact]
    public void SetGroup_ShouldUpdateGroupIdAndUpdatedAt()
    {
        // Arrange
        var user = new User("John", "Doe", "john@example.com", "hash", 1, 1);
        var oldUpdatedAt = user.UpdatedAt;
        var newGroupId = 5;

        // Act
        user.SetGroup(newGroupId);

        // Assert
        user.GroupId.Should().Be(newGroupId);
        user.UpdatedAt.Should().BeOnOrAfter(oldUpdatedAt);
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyCollections()
    {
        // Arrange & Act
        var user = new User("John", "Doe", "john@example.com", "hash", 1, 1);

        // Assert
        user.ConfirmationCodes.Should().NotBeNull().And.BeEmpty();
        user.PasswordResetCodes.Should().NotBeNull().And.BeEmpty();
        user.RefreshTokens.Should().NotBeNull().And.BeEmpty();
    }
}

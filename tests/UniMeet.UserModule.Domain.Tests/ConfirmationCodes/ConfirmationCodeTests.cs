using FluentAssertions;
using UniMeet.UserModule.Domain.ConfirmationCodes;

namespace UniMeet.UserModule.Domain.Tests.ConfirmationCodes;

public class ConfirmationCodeTests
{
    [Fact]
    public void Constructor_ShouldCreateConfirmationCodeWithValidProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(24);

        // Act
        var confirmationCode = new ConfirmationCode(userId, expiresAt);

        // Assert
        confirmationCode.UserId.Should().Be(userId);
        confirmationCode.Code.Should().NotBe(Guid.Empty);
        confirmationCode.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        confirmationCode.ExpiresAtUtc.Should().Be(expiresAt);
    }

    [Fact]
    public void IsExpired_WhenNotExpired_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var confirmationCode = new ConfirmationCode(userId, expiresAt);

        // Act
        var result = confirmationCode.IsExpired();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsExpired_WhenExpired_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(-1);
        var confirmationCode = new ConfirmationCode(userId, expiresAt);

        // Act
        var result = confirmationCode.IsExpired();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCorrect_WithMatchingCode_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(24);
        var confirmationCode = new ConfirmationCode(userId, expiresAt);

        // Act
        var result = confirmationCode.IsCorrect(confirmationCode.Code);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCorrect_WithNonMatchingCode_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(24);
        var confirmationCode = new ConfirmationCode(userId, expiresAt);
        var differentCode = Guid.NewGuid();

        // Act
        var result = confirmationCode.IsCorrect(differentCode);

        // Assert
        result.Should().BeFalse();
    }
}

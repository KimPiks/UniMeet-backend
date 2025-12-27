using FluentAssertions;
using UniMeet.UserModule.Domain.PasswordResetCodes;

namespace UniMeet.UserModule.Domain.Tests.PasswordResetCodes;

public class PasswordResetCodeTests
{
    [Fact]
    public void Constructor_ShouldCreatePasswordResetCodeWithValidProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(1);

        // Act
        var resetCode = new PasswordResetCode(userId, expiresAt);

        // Assert
        resetCode.UserId.Should().Be(userId);
        resetCode.Code.Should().NotBe(Guid.Empty);
        resetCode.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        resetCode.ExpiresAtUtc.Should().Be(expiresAt);
    }

    [Fact]
    public void IsExpired_WhenNotExpired_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var resetCode = new PasswordResetCode(userId, expiresAt);

        // Act
        var result = resetCode.IsExpired();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsExpired_WhenExpired_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(-1);
        var resetCode = new PasswordResetCode(userId, expiresAt);

        // Act
        var result = resetCode.IsExpired();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCorrect_WithMatchingCode_ShouldReturnTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var resetCode = new PasswordResetCode(userId, expiresAt);

        // Act
        var result = resetCode.IsCorrect(resetCode.Code);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCorrect_WithNonMatchingCode_ShouldReturnFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var resetCode = new PasswordResetCode(userId, expiresAt);
        var differentCode = Guid.NewGuid();

        // Act
        var result = resetCode.IsCorrect(differentCode);

        // Assert
        result.Should().BeFalse();
    }
}

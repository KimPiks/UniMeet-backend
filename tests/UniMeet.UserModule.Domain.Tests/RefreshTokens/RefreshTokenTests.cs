using FluentAssertions;
using UniMeet.UserModule.Domain.RefreshTokens;

namespace UniMeet.UserModule.Domain.Tests.RefreshTokens;

public class RefreshTokenTests
{
    [Fact]
    public void Constructor_ShouldCreateRefreshTokenWithValidProperties()
    {
        // Arrange
        var token = "sample_refresh_token";
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var userId = Guid.NewGuid();

        // Act
        var refreshToken = new RefreshToken(token, expiresAt, userId);

        // Assert
        refreshToken.Token.Should().Be(token);
        refreshToken.UserId.Should().Be(userId);
        refreshToken.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        refreshToken.ExpiresAtUtc.Should().Be(expiresAt);
    }

    [Fact]
    public void IsExpired_WhenNotExpired_ShouldReturnFalse()
    {
        // Arrange
        var token = "sample_refresh_token";
        var expiresAt = DateTime.UtcNow.AddDays(1);
        var userId = Guid.NewGuid();
        var refreshToken = new RefreshToken(token, expiresAt, userId);

        // Act
        var result = refreshToken.IsExpired();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsExpired_WhenExpired_ShouldReturnTrue()
    {
        // Arrange
        var token = "sample_refresh_token";
        var expiresAt = DateTime.UtcNow.AddDays(-1);
        var userId = Guid.NewGuid();
        var refreshToken = new RefreshToken(token, expiresAt, userId);

        // Act
        var result = refreshToken.IsExpired();

        // Assert
        result.Should().BeTrue();
    }
}

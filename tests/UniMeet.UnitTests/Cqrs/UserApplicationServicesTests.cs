using System.IdentityModel.Tokens.Jwt;
using ModularSystem.Auth;
using UniMeet.UserModule.Application.Services;
using UniMeet.UserModule.Domain.Models;

namespace UniMeet.UnitTests.Cqrs;

public class UserApplicationServicesTests
{
    private const string JwtSecret = "this-secret-key-is-long-enough-for-hs256-tests";

    [Fact]
    public void JwtService_generates_distinct_token_types_and_reads_expected_user_id()
    {
        var userId = Guid.NewGuid();
        var service = new JwtService(JwtSecret, "issuer", "audience");

        var accessToken = service.GenerateAccessToken(userId, true, 4, ["Users.Read", "Users.Read", "Users.Write"]);
        var refreshToken = service.GenerateRefreshToken(userId);
        var refreshUserId = service.GetUserIdFromToken(refreshToken, TokenType.Refresh);
        var accessJwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

        Assert.Equal(userId, refreshUserId);
        Assert.Equal(AuthTokenTypes.Access, accessJwt.Claims.Single(claim => claim.Type == AuthClaimTypes.TokenType).Value);
        Assert.Equal(["Users.Read", "Users.Write"], accessJwt.Claims
            .Where(claim => claim.Type == AuthClaimTypes.Permission)
            .Select(claim => claim.Value)
            .Order()
            .ToArray());
        Assert.NotEqual(accessToken, refreshToken);
        Assert.Throws<UnauthorizedAccessException>(() => service.GetUserIdFromToken(accessToken, TokenType.Refresh));
    }

    [Fact]
    public void JwtService_returns_future_expiration_for_each_token_type()
    {
        var service = new JwtService(JwtSecret, "issuer", "audience");

        var accessExpiration = service.GetExpires(TokenType.Access);
        var refreshExpiration = service.GetExpires(TokenType.Refresh);

        Assert.True(accessExpiration > DateTime.UtcNow);
        Assert.True(refreshExpiration > accessExpiration);
    }

    [Fact]
    public void Link_services_build_expected_user_urls()
    {
        var code = Guid.NewGuid();
        var confirmation = new ConfirmationLinkService("https://example.test");
        var passwordReset = new PasswordResetLinkService("https://example.test");

        var confirmationUrl = confirmation.Create(code);
        var resetUrl = passwordReset.Create(code);

        Assert.Equal($"https://example.test/User/ConfirmAccount/?code={code}", confirmationUrl);
        Assert.Equal($"https://example.test/User/PasswordReset/?code={code}", resetUrl);
    }

    [Fact]
    public void Password_hasher_verifies_original_password_and_rejects_different_value()
    {
        var hasher = new BCryptPasswordHasher();

        var hash = hasher.Hash("Passw0rd!");

        Assert.True(hasher.Verify("Passw0rd!", hash));
        Assert.False(hasher.Verify("other", hash));
    }

    [Fact]
    public void Profile_picture_validator_rejects_invalid_size_type_extension_and_content()
    {
        var validator = new ProfilePictureValidator();
        var tooLarge = new byte[(5 * 1024 * 1024) + 1];

        var size = validator.ValidateProfilePicture(tooLarge, "avatar.jpg", "image/jpeg");
        var type = validator.ValidateProfilePicture([1, 2, 3], "avatar.jpg", "application/pdf");
        var extension = validator.ValidateProfilePicture([1, 2, 3], "avatar.gif", "image/jpeg");
        var content = validator.ValidateProfilePicture([1, 2, 3], "avatar.jpg", "image/jpeg");

        Assert.False(size.IsValid);
        Assert.Contains("File size exceeds", size.ErrorMessage);
        Assert.False(type.IsValid);
        Assert.Contains("Invalid file type", type.ErrorMessage);
        Assert.False(extension.IsValid);
        Assert.Contains("Invalid file extension", extension.ErrorMessage);
        Assert.False(content.IsValid);
        Assert.Contains("Failed to validate image format", content.ErrorMessage);
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ModularSystem.Auth;
using UniMeet.UserModule.Domain.Models;
using UniMeet.UserModule.Domain.Services;

namespace UniMeet.UserModule.Application.Services;

public class JwtService : IJwtService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    
    private const int AccessTokenExpiresMinutes = 15;
    private const int RefreshTokenExpireMinutes = 1440;
    
    public JwtService(string secretKey, string issuer, string audience)
    {
        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
    }
    
    public string GenerateAccessToken(Guid userId, bool isActive, int groupId, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(AuthClaimTypes.TokenType, AuthTokenTypes.Access),
            new(AuthClaimTypes.IsActive, isActive.ToString()),
            new(AuthClaimTypes.GroupId, groupId.ToString())
        };

        claims.AddRange(permissions.Distinct().Select(permission => new Claim(AuthClaimTypes.Permission, permission)));
        return GenerateToken(claims, TokenType.Access);
    }

    public string GenerateRefreshToken(Guid userId)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(AuthClaimTypes.TokenType, AuthTokenTypes.Refresh)
        };

        return GenerateToken(claims, TokenType.Refresh);
    }

    private string GenerateToken(IEnumerable<Claim> claims, TokenType tokenType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = this.GetExpires(tokenType),
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public Guid GetUserIdFromToken(string token, TokenType expectedTokenType)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
                LifetimeValidator = (notBefore, expires, _, _) =>
                {
                    var currentTime = DateTime.UtcNow;
                    return (notBefore == null || notBefore <= currentTime) &&
                           (expires == null || expires >= currentTime);
                }
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            var tokenType = principal.FindFirst(AuthClaimTypes.TokenType)?.Value;
            if (tokenType != expectedTokenType.ToString())
                throw new UnauthorizedAccessException("Invalid token type");

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new UnauthorizedAccessException("Invalid token claims");

            return Guid.Parse(claim.Value);
        }
        catch (Exception)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }
    }
    
    public DateTime GetExpires(TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.Access => DateTime.UtcNow.AddMinutes(AccessTokenExpiresMinutes),
            TokenType.Refresh => DateTime.UtcNow.AddMinutes(RefreshTokenExpireMinutes),
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
        };
    }
}

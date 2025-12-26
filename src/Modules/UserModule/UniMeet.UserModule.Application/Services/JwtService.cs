using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UniMeet.UserModule.Domain.Jwt.Exceptions;
using UniMeet.UserModule.Domain.Models;
using UniMeet.UserModule.Domain.Services;

namespace UniMeet.UserModule.Application.Services;

public class JwtService : IJwtService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    
    private const int AccessTokenExpiresMinutes = 1;
    private const int RefreshTokenExpireMinutes = 10;
    
    public JwtService(string secretKey, string issuer, string audience)
    {
        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
    }
    
    public string GenerateToken(Guid userId, TokenType tokenType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }),
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

    public Guid GetUserIdFromToken(string token)
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
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new JwtException("Invalid token (claims not found)");

            return Guid.Parse(claim.Value);
        }
        catch (Exception e)
        {
            throw new JwtException("Invalid token");
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
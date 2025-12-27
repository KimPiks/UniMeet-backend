using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Domain.RefreshTokens;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    private RefreshToken() { }
    
    public RefreshToken(string token, DateTime expiresAtUtc, Guid userId)
    {
        Token = token;
        CreatedAtUtc = DateTime.UtcNow;
        ExpiresAtUtc = expiresAtUtc;
        UserId = userId;
    }
    
    public bool IsExpired() => DateTime.UtcNow > ExpiresAtUtc;
}
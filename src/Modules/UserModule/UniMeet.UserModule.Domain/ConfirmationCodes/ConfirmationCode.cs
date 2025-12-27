using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Domain.ConfirmationCodes;

public class ConfirmationCode
{
    public int Id { get; set; }
    public Guid Code { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    private ConfirmationCode() { }

    public ConfirmationCode(Guid userId, DateTime expiresAt)
    {
        UserId = userId;
        Code = Guid.NewGuid();
        CreatedAtUtc = DateTime.UtcNow;
        ExpiresAtUtc = expiresAt;
    }
    
    public bool IsExpired() => DateTime.UtcNow > ExpiresAtUtc;
    public bool IsCorrect(Guid code) => code == Code;
}
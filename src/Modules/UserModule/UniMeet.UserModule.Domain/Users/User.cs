using UniMeet.UserModule.Domain.ConfirmationCodes;
using UniMeet.UserModule.Domain.PasswordResetCodes;
using UniMeet.UserModule.Domain.RefreshTokens;

namespace UniMeet.UserModule.Domain.Users;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set;} = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = false;
    public int GroupId { get; private set; }

    public int UniversityId { get; set; }
    
    public List<ConfirmationCode> ConfirmationCodes { get; set; } = new();
    public List<PasswordResetCode> PasswordResetCodes { get; set; } = new();
    public List<RefreshToken> RefreshTokens { get; set; } = new(); 

    private User() { }

    public User(string firstName, string lastName, string email, string passwordHash, int universityId, int groupId)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        UniversityId = universityId;
        IsActive = false;
        GroupId = groupId;
        
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Rename(string newFirstName, string newLastName)
    {
        FirstName = newFirstName;
        LastName = newLastName;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetGroup(int groupId)
    {
        GroupId = groupId;
        UpdatedAt = DateTime.UtcNow;
    }
}
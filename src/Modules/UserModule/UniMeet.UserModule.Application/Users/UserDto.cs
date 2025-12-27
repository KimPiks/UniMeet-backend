namespace UniMeet.UserModule.Application.Users;

public class UserDto
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = false;
    public required int UniversityId { get; set; }
    public required int GroupId { get; set; }
}
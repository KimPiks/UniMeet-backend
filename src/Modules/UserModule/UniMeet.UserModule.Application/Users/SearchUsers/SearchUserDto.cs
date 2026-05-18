using UniMeet.UserModule.Application.Interests;

namespace UniMeet.UserModule.Application.Users.SearchUsers;

public class SearchUserDto
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required int UniversityId { get; set; }
    public bool IsActive { get; set; }
    public string Sex { get; set; } = string.Empty;
    public List<InterestDto> Interests { get; set; } = new();
    public int Score { get; set; }
    public int? FieldOfStudyId { get; set; }
}


using ModularSystem.Contracts.User.Interests;

namespace ModularSystem.Contracts.User.UserDetails;

public class UserDetailDto
{
    public required int Id { get; set; }
    public required Guid UserId { get; set; }
    public required string Sex { get; set; }
    public List<InterestDto> Interests { get; set; } = new();
    public string? ProfilePicturePath { get; set; }
    public string? ProfilePictureMimeType { get; set; }
}





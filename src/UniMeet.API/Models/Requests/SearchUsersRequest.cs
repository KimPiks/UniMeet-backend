using UniMeet.UserModule.Domain.UserDetails;

namespace UniMeet.API.Models.Requests;

public class SearchUsersRequest
{
    public required UserSearchProfileRequest Profile { get; set; }
    public UserSearchFiltersRequest? Filters { get; set; }
    public int Offset { get; set; } = 0;
    public int Padding { get; set; } = 50;
}

public class UserSearchProfileRequest
{
    public int UniversityId { get; set; }
    public int FieldOfStudyId { get; set; }
    public List<int> InterestIds { get; set; } = new();
    public Sex Sex { get; set; }
}

public class UserSearchFiltersRequest
{
    public int? UniversityId { get; set; }
    public int? FieldOfStudyId { get; set; }
    public List<int>? InterestIds { get; set; }
    public Sex? Sex { get; set; }
}


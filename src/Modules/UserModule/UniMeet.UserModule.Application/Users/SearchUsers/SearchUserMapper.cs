using UniMeet.UserModule.Application.Interests;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Application.Users.SearchUsers;

public static class SearchUserMapper
{
    public static SearchUserDto ToSearchDto(this User user, int score, int? fieldOfStudyId)
    {
        return new SearchUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UniversityId = user.UniversityId,
            IsActive = user.IsActive,
            Sex = user.UserDetail?.Sex.ToString() ?? string.Empty,
            Interests = user.UserDetail?.Interests.Select(i => i.ToDto()).ToList() ?? new List<InterestDto>(),
            Score = score,
            FieldOfStudyId = fieldOfStudyId
        };
    }
}


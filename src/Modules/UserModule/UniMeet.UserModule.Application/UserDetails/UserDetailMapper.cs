using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Application.Interests;

namespace UniMeet.UserModule.Application.UserDetails;

public static class UserDetailMapper
{
    public static UserDetailDto ToDto(this UserDetail userDetail)
    {
        return new UserDetailDto
        {
            Id = userDetail.Id,
            UserId = userDetail.UserId,
            Sex = userDetail.Sex.ToString(),
            Interests = userDetail.Interests.Select(i => i.ToDto()).ToList(),
            ProfilePicturePath = userDetail.ProfilePicturePath,
            ProfilePictureMimeType = userDetail.ProfilePictureMimeType
        };
    }
}






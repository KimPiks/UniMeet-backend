using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Application.Users;

public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UniversityId = user.UniversityId,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            GroupId = user.GroupId
        };
    }
}
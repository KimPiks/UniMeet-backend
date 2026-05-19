using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Application.Users.GetUserAccessInfo;

public class GetUserAccessInfoQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserAccessInfoQuery, UserAccessInfoDto?>
{
    public async Task<UserAccessInfoDto?> HandleAsync(
        GetUserAccessInfoQuery request,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        return user is null ? null : new UserAccessInfoDto(user.Id, user.IsActive, user.GroupId);
    }
}

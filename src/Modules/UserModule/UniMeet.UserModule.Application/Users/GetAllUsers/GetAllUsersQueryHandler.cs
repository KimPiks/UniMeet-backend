using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Application.Users.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository) : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> HandleAsync(GetAllUsersQuery request, CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(request.Offset, request.Limit, cancellationToken);
        return users.Select(u => u.ToDto());
    }
}
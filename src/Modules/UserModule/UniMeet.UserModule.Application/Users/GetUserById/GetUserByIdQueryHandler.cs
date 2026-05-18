using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Application.Users.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> HandleAsync(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null) return null;
        return user.ToDto();
    }
}


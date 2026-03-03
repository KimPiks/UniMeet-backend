using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.UserDetails;

namespace UniMeet.UserModule.Application.UserDetails.GetUserDetailByUserId;

public class GetUserDetailByUserIdQueryHandler(IUserDetailRepository userDetailRepository) 
    : IQueryHandler<GetUserDetailByUserIdQuery, UserDetailDto?>
{
    public async Task<UserDetailDto?> HandleAsync(GetUserDetailByUserIdQuery request, CancellationToken cancellationToken = default)
    {
        var userDetail = await userDetailRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        return userDetail?.ToDto();
    }
}


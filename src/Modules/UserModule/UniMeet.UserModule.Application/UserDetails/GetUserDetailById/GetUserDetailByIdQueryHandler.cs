using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.UserDetails;

namespace UniMeet.UserModule.Application.UserDetails.GetUserDetailById;

public class GetUserDetailByIdQueryHandler(IUserDetailRepository userDetailRepository) 
    : IQueryHandler<GetUserDetailByIdQuery, UserDetailDto?>
{
    public async Task<UserDetailDto?> HandleAsync(GetUserDetailByIdQuery request, CancellationToken cancellationToken = default)
    {
        var userDetail = await userDetailRepository.GetByIdAsync(request.UserDetailId, cancellationToken);
        return userDetail?.ToDto();
    }
}


using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.UserDetails.GetUserDetailByUserId;

public record GetUserDetailByUserIdQuery(Guid UserId) : IQuery<UserDetailDto?>;


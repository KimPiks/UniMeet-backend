using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.UserDetails.GetUserDetailById;

public record GetUserDetailByIdQuery(int UserDetailId) : IQuery<UserDetailDto?>;


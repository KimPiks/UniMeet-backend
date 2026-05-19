using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.UserDetails.GetUserDetailByUserId;

public record GetUserDetailByUserIdQuery(Guid UserId) : IQuery<UserDetailDto?>;


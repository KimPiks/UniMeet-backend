using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.UserDetails.GetUserDetailById;

public record GetUserDetailByIdQuery(int UserDetailId) : IQuery<UserDetailDto?>;


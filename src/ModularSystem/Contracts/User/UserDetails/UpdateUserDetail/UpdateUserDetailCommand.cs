using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.UserDetails.UpdateUserDetail;

public record UpdateUserDetailCommand(int UserDetailId, Guid RequestingUserId, List<int>? InterestIds) : ICommand<UserDetailDto>;

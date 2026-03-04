using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.UserDetails.UpdateUserDetail;

public record UpdateUserDetailCommand(int UserDetailId, Guid RequestingUserId, List<int>? InterestIds) : ICommand<UserDetailDto>;

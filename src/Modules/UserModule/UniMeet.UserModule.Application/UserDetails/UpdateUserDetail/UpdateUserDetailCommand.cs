using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.UserDetails.UpdateUserDetail;

public record UpdateUserDetailCommand(int UserDetailId, List<int>? InterestIds) : ICommand<UserDetailDto>;

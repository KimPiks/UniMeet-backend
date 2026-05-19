using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.UserDetails.DeleteProfilePicture;

public record DeleteProfilePictureCommand(int UserDetailId, Guid RequestingUserId) : ICommand<UserDetailDto>;


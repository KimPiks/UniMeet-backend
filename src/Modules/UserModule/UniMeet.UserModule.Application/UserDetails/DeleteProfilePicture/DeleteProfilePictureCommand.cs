using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.UserDetails.DeleteProfilePicture;

public record DeleteProfilePictureCommand(int UserDetailId) : ICommand<UserDetailDto>;


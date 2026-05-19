using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.UserDetails.UploadProfilePicture;

public record UploadProfilePictureCommand(
    int UserDetailId,
    Guid UserId,
    byte[] FileContent, 
    string FileName, 
    string MimeType) : ICommand<UserDetailDto>;



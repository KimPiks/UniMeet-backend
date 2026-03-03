using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.UserDetails.UploadProfilePicture;

public record UploadProfilePictureCommand(
    int UserDetailId,
    Guid UserId,
    byte[] FileContent, 
    string FileName, 
    string MimeType) : ICommand<UserDetailDto>;



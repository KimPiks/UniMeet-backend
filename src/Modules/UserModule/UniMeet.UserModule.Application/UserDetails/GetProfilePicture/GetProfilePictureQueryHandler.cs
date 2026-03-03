using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Application.Services;

namespace UniMeet.UserModule.Application.UserDetails.GetProfilePicture;

public class GetProfilePictureQueryHandler(
    IUserDetailRepository userDetailRepository,
    IFileStorageService fileStorageService) 
    : IQueryHandler<GetProfilePictureQuery, ProfilePictureDto>
{
    public async Task<ProfilePictureDto> HandleAsync(GetProfilePictureQuery request, CancellationToken cancellationToken = default)
    {
        var userDetail = await userDetailRepository.GetByIdAsync(request.UserDetailId, cancellationToken);
        if (userDetail == null)
        {
            throw new KeyNotFoundException($"UserDetail with id {request.UserDetailId} not found");
        }

        if (string.IsNullOrEmpty(userDetail.ProfilePicturePath) || string.IsNullOrEmpty(userDetail.ProfilePictureMimeType))
        {
            throw new KeyNotFoundException($"UserDetail with id {request.UserDetailId} has no profile picture");
        }

        // Read file from disk
        var pictureData = await fileStorageService.ReadFileAsync(userDetail.ProfilePicturePath, cancellationToken);

        // Determine file extension based on MIME type
        var extension = userDetail.ProfilePictureMimeType.ToLower() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            _ => ".jpg"
        };

        return new ProfilePictureDto
        {
            PictureData = pictureData,
            MimeType = userDetail.ProfilePictureMimeType,
            FileName = $"profile-picture-{userDetail.Id}{extension}"
        };
    }
}



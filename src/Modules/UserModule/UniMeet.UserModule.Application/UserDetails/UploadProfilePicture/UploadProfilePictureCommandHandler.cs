using UniMeet.Shared.Abstractions;
using UniMeet.Shared.Exceptions;
using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Application.Services;

namespace UniMeet.UserModule.Application.UserDetails.UploadProfilePicture;

public class UploadProfilePictureCommandHandler(
    IUserDetailRepository userDetailRepository, 
    IProfilePictureValidator profilePictureValidator,
    IFileStorageService fileStorageService) 
    : ICommandHandler<UploadProfilePictureCommand, UserDetailDto>
{
    public async Task<UserDetailDto> HandleAsync(UploadProfilePictureCommand request, CancellationToken cancellationToken = default)
    {
        // Validate profile picture
        var (isValid, errorMessage) = profilePictureValidator.ValidateProfilePicture(
            request.FileContent, 
            request.FileName, 
            request.MimeType);
        
        if (!isValid)
        {
            throw new ValidationException(errorMessage ?? "Profile picture validation failed");
        }

        // Get user detail
        var userDetail = await userDetailRepository.GetByIdAsync(request.UserDetailId, cancellationToken);
        if (userDetail == null)
        {
            throw new ValidationException($"UserDetail with id {request.UserDetailId} not found");
        }

        if (userDetail.UserId != request.UserId)
            throw new ForbiddenException("You are not allowed to upload a profile picture for another user.");

        // Delete old picture if exists
        if (!string.IsNullOrEmpty(userDetail.ProfilePicturePath))
        {
            await fileStorageService.DeleteProfilePictureAsync(userDetail.ProfilePicturePath, cancellationToken);
        }

        // Save file to disk
        var relativePath = await fileStorageService.SaveProfilePictureAsync(
            request.FileContent, 
            request.FileName, 
            request.UserId, 
            cancellationToken);

        // Update entity with file path
        userDetail.SetProfilePicture(relativePath, request.MimeType);

        // Save changes to database
        await userDetailRepository.SaveChangesAsync(cancellationToken);

        return userDetail.ToDto();
    }
}




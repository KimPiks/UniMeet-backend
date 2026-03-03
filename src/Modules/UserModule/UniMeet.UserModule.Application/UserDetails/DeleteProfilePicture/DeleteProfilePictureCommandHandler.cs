using UniMeet.Shared.Abstractions;
using UniMeet.UserModule.Domain.UserDetails;
using UniMeet.UserModule.Application.Services;

namespace UniMeet.UserModule.Application.UserDetails.DeleteProfilePicture;

public class DeleteProfilePictureCommandHandler(
    IUserDetailRepository userDetailRepository,
    IFileStorageService fileStorageService) 
    : ICommandHandler<DeleteProfilePictureCommand, UserDetailDto>
{
    public async Task<UserDetailDto> HandleAsync(DeleteProfilePictureCommand request, CancellationToken cancellationToken = default)
    {
        // Get user detail
        var userDetail = await userDetailRepository.GetByIdAsync(request.UserDetailId, cancellationToken);
        if (userDetail == null)
        {
            throw new KeyNotFoundException($"UserDetail with id {request.UserDetailId} not found");
        }

        // Delete file from disk if exists
        if (!string.IsNullOrEmpty(userDetail.ProfilePicturePath))
        {
            await fileStorageService.DeleteProfilePictureAsync(userDetail.ProfilePicturePath, cancellationToken);
        }

        // Remove profile picture reference
        userDetail.RemoveProfilePicture();

        // Save changes
        await userDetailRepository.SaveChangesAsync(cancellationToken);

        return userDetail.ToDto();
    }
}



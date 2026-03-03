using UniMeet.Shared.Abstractions;

namespace UniMeet.UserModule.Application.UserDetails.GetProfilePicture;

public record GetProfilePictureQuery(int UserDetailId) : IQuery<ProfilePictureDto>;


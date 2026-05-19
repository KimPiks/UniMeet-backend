using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.User.UserDetails.GetProfilePicture;

public record GetProfilePictureQuery(int UserDetailId) : IQuery<ProfilePictureDto>;


namespace UniMeet.UserModule.Application.UserDetails.GetProfilePicture;

public class ProfilePictureDto
{
    public required byte[] PictureData { get; set; }
    public required string MimeType { get; set; }
    public required string FileName { get; set; }
}


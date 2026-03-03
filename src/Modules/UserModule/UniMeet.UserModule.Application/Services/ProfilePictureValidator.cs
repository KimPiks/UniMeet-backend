using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace UniMeet.UserModule.Application.Services;

public interface IProfilePictureValidator
{
    /// <summary>
    /// Validates profile picture file
    /// </summary>
    /// <param name="fileContent">File content as byte array</param>
    /// <param name="fileName">Original file name</param>
    /// <param name="mimeType">MIME type of the file</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    (bool IsValid, string? ErrorMessage) ValidateProfilePicture(byte[] fileContent, string fileName, string mimeType);
}

public class ProfilePictureValidator : IProfilePictureValidator
{
    // Configuration constants
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
    private const int MinWidth = 200;
    private const int MaxWidth = 4000;
    private const int MinHeight = 200;
    private const int MaxHeight = 4000;
    
    private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/png" };
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

    public (bool IsValid, string? ErrorMessage) ValidateProfilePicture(byte[] fileContent, string fileName, string mimeType)
    {
        // Validate file size
        if (fileContent.Length > MaxFileSizeBytes)
        {
            return (false, $"File size exceeds maximum allowed size of {MaxFileSizeBytes / (1024 * 1024)} MB");
        }

        // Validate MIME type
        if (!AllowedMimeTypes.Contains(mimeType.ToLower()))
        {
            return (false, $"Invalid file type. Only JPG and PNG files are allowed. Provided: {mimeType}");
        }

        // Validate file extension
        var fileExtension = Path.GetExtension(fileName).ToLower();
        if (!AllowedExtensions.Contains(fileExtension))
        {
            return (false, $"Invalid file extension. Only JPG and PNG files are allowed. Provided: {fileExtension}");
        }

        // Validate image dimensions
        try
        {
            using (var image = Image.Load(fileContent))
            {
                if (image.Width < MinWidth || image.Width > MaxWidth)
                {
                    return (false, $"Image width must be between {MinWidth} and {MaxWidth} pixels. Provided: {image.Width}");
                }

                if (image.Height < MinHeight || image.Height > MaxHeight)
                {
                    return (false, $"Image height must be between {MinHeight} and {MaxHeight} pixels. Provided: {image.Height}");
                }
            }
        }
        catch (Exception ex)
        {
            return (false, $"Failed to validate image format: {ex.Message}");
        }

        return (true, null);
    }
}


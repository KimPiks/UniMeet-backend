using Microsoft.AspNetCore.Hosting;
using UniMeet.UserModule.Application.Services;

namespace UniMeet.UserModule.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private const string ProfilePicturesDir = "uploads/profile-pictures";

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public async Task<string> SaveProfilePictureAsync(byte[] fileContent, string fileName, Guid userId, CancellationToken cancellationToken = default)
    {
        if (fileContent == null || fileContent.Length == 0)
            throw new ArgumentException("File content is empty", nameof(fileContent));

        // Create directory structure: uploads/profile-pictures/userId
        var userDir = Path.Combine(_environment.ContentRootPath, ProfilePicturesDir, userId.ToString());
        Directory.CreateDirectory(userDir);

        // Generate unique filename
        var fileExtension = Path.GetExtension(fileName).ToLower();
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var fullPath = Path.Combine(userDir, uniqueFileName);

        // Delete old profile picture if exists
        var existingFiles = Directory.GetFiles(userDir);
        foreach (var oldFile in existingFiles)
        {
            try
            {
                File.Delete(oldFile);
            }
            catch
            {
                // Log if needed, but continue
            }
        }

        // Save new file
        await File.WriteAllBytesAsync(fullPath, fileContent, cancellationToken);

        // Return relative path
        return Path.Combine(ProfilePicturesDir, userId.ToString(), uniqueFileName).Replace("\\", "/");
    }

    public async Task<bool> DeleteProfilePictureAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(filePath))
            return false;

        var fullPath = ResolveProfilePicturePath(filePath);

        if (!File.Exists(fullPath))
            return false;

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            File.Delete(fullPath);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool FileExists(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return false;

        var fullPath = ResolveProfilePicturePath(filePath);
        return File.Exists(fullPath);
    }

    public async Task<byte[]> ReadFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path is empty", nameof(filePath));

        var fullPath = ResolveProfilePicturePath(filePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"File not found: {filePath}");

        return await File.ReadAllBytesAsync(fullPath, cancellationToken);
    }

    public string GetUploadDirectory()
    {
        return GetProfilePicturesRoot();
    }

    private string ResolveProfilePicturePath(string filePath)
    {
        var root = GetProfilePicturesRoot();
        var fullPath = Path.GetFullPath(Path.Combine(_environment.ContentRootPath, filePath));
        var rootWithSeparator = root.EndsWith(Path.DirectorySeparatorChar)
            ? root
            : root + Path.DirectorySeparatorChar;

        if (!fullPath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Invalid profile picture path.");
        }

        return fullPath;
    }

    private string GetProfilePicturesRoot()
        => Path.GetFullPath(Path.Combine(_environment.ContentRootPath, ProfilePicturesDir));
}




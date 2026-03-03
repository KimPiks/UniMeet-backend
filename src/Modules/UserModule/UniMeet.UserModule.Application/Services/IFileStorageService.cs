namespace UniMeet.UserModule.Application.Services;

public interface IFileStorageService
{
    /// <summary>
    /// Saves uploaded file to disk
    /// </summary>
    /// <param name="fileContent">File content as byte array</param>
    /// <param name="fileName">Original file name</param>
    /// <param name="userId">User ID for organizing files</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Relative path to saved file</returns>
    Task<string> SaveProfilePictureAsync(byte[] fileContent, string fileName, Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes file from disk
    /// </summary>
    /// <param name="filePath">Relative path to file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<bool> DeleteProfilePictureAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if file exists on disk
    /// </summary>
    /// <param name="filePath">Relative path to file</param>
    bool FileExists(string filePath);

    /// <summary>
    /// Reads file from disk
    /// </summary>
    /// <param name="filePath">Relative path to file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<byte[]> ReadFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets absolute path to upload directory
    /// </summary>
    string GetUploadDirectory();
}


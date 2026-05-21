using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using UniMeet.UserModule.Infrastructure.Services;

namespace UniMeet.UnitTests.Infrastructure;

public class FileStorageServiceTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), $"UniMeet-tests-{Guid.NewGuid()}");
    private readonly FileStorageService _service;

    public FileStorageServiceTests()
    {
        Directory.CreateDirectory(_root);
        _service = new FileStorageService(new FakeWebHostEnvironment(_root));
    }

    [Fact]
    public async Task SaveProfilePictureAsync_writes_file_under_profile_picture_root()
    {
        var userId = Guid.NewGuid();
        var content = new byte[] { 1, 2, 3 };

        var relativePath = await _service.SaveProfilePictureAsync(content, "avatar.PNG", userId);

        Assert.StartsWith($"uploads/profile-pictures/{userId}/", relativePath);
        Assert.EndsWith(".png", relativePath);
        Assert.True(_service.FileExists(relativePath));
        Assert.Equal(content, await _service.ReadFileAsync(relativePath));
    }

    [Fact]
    public async Task DeleteProfilePictureAsync_removes_only_files_inside_profile_picture_root()
    {
        var userId = Guid.NewGuid();
        var relativePath = await _service.SaveProfilePictureAsync([9, 8, 7], "avatar.png", userId);

        var deleted = await _service.DeleteProfilePictureAsync(relativePath);

        Assert.True(deleted);
        Assert.False(_service.FileExists(relativePath));
    }

    [Fact]
    public async Task File_operations_reject_paths_outside_profile_picture_root()
    {
        var outsidePath = Path.Combine(_root, "secret.txt");
        await File.WriteAllTextAsync(outsidePath, "do-not-read");
        const string traversalPath = "uploads/profile-pictures/../../secret.txt";

        Assert.Throws<UnauthorizedAccessException>(() => _service.FileExists(traversalPath));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.ReadFileAsync(traversalPath));
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteProfilePictureAsync(traversalPath));
    }

    public void Dispose()
    {
        if (Directory.Exists(_root))
        {
            Directory.Delete(_root, recursive: true);
        }
    }

    private sealed class FakeWebHostEnvironment(string root) : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; } = "UnitTests";
        public string ApplicationName { get; set; } = "UniMeet.UnitTests";
        public string WebRootPath { get; set; } = root;
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = root;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}

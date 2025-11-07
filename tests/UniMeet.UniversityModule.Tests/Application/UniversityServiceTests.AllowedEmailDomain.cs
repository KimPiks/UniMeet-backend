using Moq;
using Xunit;
using System.Threading;
using FluentAssertions;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Tests.Application;

public partial class UniversityServiceTests
{
    // --- Tests for AddAllowedEmailDomainAsync ---

    [Fact]
    public async Task AddAllowedEmailDomainAsync_ShouldAddDomainAndSaveChanges_WhenUniversityExists()
    {
        // ---- ARRANGE ----
        var domain = "test.edu";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.AddAllowedEmailDomainAsync(1, domain);

        // ---- ASSERT ----
        fakeUniversity.AllowedEmailDomains.Should().HaveCount(1);
        fakeUniversity.AllowedEmailDomains.First().Domain.Should().Be(domain);
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task AddAllowedEmailDomainAsync_ShouldThrowException_WhenUniversityNotFound()
    {
        // ---- ARRANGE ----
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((University?)null);

        // ---- ACT ----
        Func<Task> act = () => _service.AddAllowedEmailDomainAsync(99, "test.edu");

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("University not found");
    }

    // --- Tests for GetAllowedEmailDomainsByUniversityIdAsync ---

    [Fact]
    public async Task GetAllowedEmailDomainsByUniversityIdAsync_ShouldReturnMappedDtos_WhenDomainsExist()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddAllowedEmailDomain("test1.edu", 1);
        fakeUniversity.AddAllowedEmailDomain("test2.edu", 1);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        var result = await _service.GetAllowedEmailDomainsByUniversityIdAsync(1);
        var resultList = result.ToList();

        // ---- ASSERT ----
        resultList.Should().NotBeNull();
        resultList.Should().HaveCount(2);
        resultList.First().Domain.Should().Be("test1.edu");
    }

    [Fact]
    public async Task GetAllowedEmailDomainsByUniversityIdAsync_ShouldThrowException_WhenUniversityNotFound()
    {
        // ---- ARRANGE ----
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((University?)null);

        // ---- ACT ----
        Func<Task> act = () => _service.GetAllowedEmailDomainsByUniversityIdAsync(99);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("University not found");
    }

    // --- Tests for DeleteAllowedEmailDomainAsync ---

    [Fact]
    public async Task DeleteAllowedEmailDomainAsync_ShouldRemoveDomainAndSaveChanges_WhenDomainExists()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddAllowedEmailDomain("to-delete.com", 1);

        var domainToRemove = fakeUniversity.AllowedEmailDomains.First();
        typeof(AllowedEmailDomain).GetProperty("Id").SetValue(domainToRemove, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.DeleteAllowedEmailDomainAsync(1, 5);

        // ---- ASSERT ----
        fakeUniversity.AllowedEmailDomains.Should().BeEmpty();
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task DeleteAllowedEmailDomainAsync_ShouldThrowException_WhenDomainNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        Func<Task> act = () => _service.DeleteAllowedEmailDomainAsync(1, 99);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Allowed email domain not found");
    }

    // --- Tests for UpdateAllowedEmailDomainAsync ---

    [Fact]
    public async Task UpdateAllowedEmailDomainAsync_ShouldUpdateDomainAndSaveChanges_WhenDataIsProvided()
    {
        // ---- ARRANGE ----
        var oldDomain = "old.com";
        var newDomain = "new.com";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddAllowedEmailDomain(oldDomain, 1);

        var domainToUpdate = fakeUniversity.AllowedEmailDomains.First();
        typeof(AllowedEmailDomain).GetProperty("Id").SetValue(domainToUpdate, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.UpdateAllowedEmailDomainAsync(1, 5, newDomain);

        // ---- ASSERT ----
        domainToUpdate.Domain.Should().Be(newDomain);
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task UpdateAllowedEmailDomainAsync_ShouldThrowException_WhenDomainNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        Func<Task> act = () => _service.UpdateAllowedEmailDomainAsync(1, 99, "new.com");

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Allowed email domain not found");
    }
}
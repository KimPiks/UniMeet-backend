using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.UpdateAllowedEmailDomain;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;

public class UpdateAllowedEmailDomainCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly UpdateAllowedEmailDomainCommandHandler _handler;

    public UpdateAllowedEmailDomainCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new UpdateAllowedEmailDomainCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateDomainAndSaveChanges_WhenDataIsProvided()
    {
        // ---- ARRANGE ----
        var oldDomain = "old.com";
        var newDomain = "new.com";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddAllowedEmailDomain(oldDomain, 1);

        var domainToUpdate = fakeUniversity.AllowedEmailDomains.First();
        typeof(AllowedEmailDomain).GetProperty("Id")?.SetValue(domainToUpdate, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new UpdateAllowedEmailDomainCommand(1, 5, newDomain);

        // ---- ACT ----
        await _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        domainToUpdate.Domain.Should().Be(newDomain);
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDomainNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new UpdateAllowedEmailDomainCommand(1, 99, "new.com");

        // ---- ACT ----
        Func<Task> act = () => _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Allowed email domain not found");
    }
}
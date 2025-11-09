using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.AddAllowedEmailDomain;

public class AddAllowedEmailDomainCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly AddAllowedEmailDomainCommandHandler _handler;

    public AddAllowedEmailDomainCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new AddAllowedEmailDomainCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddDomainAndSaveChanges_WhenUniversityExists()
    {
        // ---- ARRANGE ----
        var domain = "test.edu";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new AddAllowedEmailDomainCommand(1, domain);

        // ---- ACT ----
        await _handler.Handle(command, CancellationToken.None);

        // ---- ASSERT ----
        fakeUniversity.AllowedEmailDomains.Should().HaveCount(1);
        fakeUniversity.AllowedEmailDomains.First().Domain.Should().Be(domain);
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUniversityNotFound()
    {
        // ---- ARRANGE ----
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((University?)null);
            
        var command = new AddAllowedEmailDomainCommand(99, "test.edu");

        // ---- ACT ----
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("University not found");
    }
}
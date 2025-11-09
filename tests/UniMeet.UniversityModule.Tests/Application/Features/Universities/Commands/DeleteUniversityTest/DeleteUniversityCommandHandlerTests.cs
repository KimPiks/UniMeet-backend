using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Universities.Commands.DeleteUniversity;

public class DeleteUniversityCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly DeleteUniversityCommandHandler _handler;

    public DeleteUniversityCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new DeleteUniversityCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteAndSaveChangesAsync_WhenUniversityExists()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Test Country", "Test Voivo", "Test City", "Test Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new DeleteUniversityCommand(1);

        // ---- ACT ----
        await _handler.Handle(command, CancellationToken.None);

        // ---- ASSERT ----
        _mockRepository.Verify(
            repo => repo.Delete(fakeUniversity), 
            Times.Once()
        );

        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenUniversityNotFound()
    {
        // ---- ARRANGE ----
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((University?)null);
            
        var command = new DeleteUniversityCommand(99);

        // ---- ACT ----
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("University not found");
                 
        _mockRepository.Verify(
            repo => repo.Delete(It.IsAny<University>()), 
            Times.Never()
        );
                 
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Never()
        );
    }
}
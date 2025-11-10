using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Universities.Commands.UpdateUniversity;

public class UpdateUniversityCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly UpdateUniversityCommandHandler _handler;

    public UpdateUniversityCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new UpdateUniversityCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateUniversityAndCallSaveChangesAsync_WhenDataIsProvided()
    {
        // ---- ARRANGE ----
        var newName = "New uni name";
        var newCountry = "New country";
        
        var fakeUniversity = new University("Old name", "Old country", "Old Voivo", "Old city", "Old addres");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        var command = new UpdateUniversityCommand(1, newName, newCountry, null, null, null);

        // ---- ACT ----
        await _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        
        fakeUniversity.Name.Should().Be(newName);
        fakeUniversity.Country.Should().Be(newCountry);

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
            
        var command = new UpdateUniversityCommand(99, "New name", null, null, null, null);

        // ---- ACT ----
        Func<Task> act = () => _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("University not found"); 
                 
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never()
        );
    }

    [Fact]
    public async Task Handle_ShouldNotUpdateUniversityButCallSaveChangesAsync_WhenDataIsNull()
    {
        // ---- ARRANGE ----
        var oldName = "Old Name";
        var fakeUniversity = new University(oldName, "Old country", "Old Voivo", "Old City", "Old addres");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new UpdateUniversityCommand(1, null, null, null, null, null);

        // ---- ACT ----
        await _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        fakeUniversity.Name.Should().Be(oldName);

        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }
}
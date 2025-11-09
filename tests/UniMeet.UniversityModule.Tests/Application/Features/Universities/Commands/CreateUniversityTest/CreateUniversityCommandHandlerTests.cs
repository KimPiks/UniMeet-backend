using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Universities.Commands.CreateUniversity;

public class CreateUniversityCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly CreateUniversityCommandHandler _handler;

    public CreateUniversityCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new CreateUniversityCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddUniversityAndCallSaveChangesAsync_WhenDataIsCorrect()
    {
        // ---- ARRANGE ----
        var name = "Test University";
        var country = "Test Country";
        var command = new CreateUniversityCommand(name, country, "Test Voivo", "Test City", "Test Address");

        // ---- ACT ----
        await _handler.Handle(command, CancellationToken.None);

        // ---- ASSERT ----
        _mockRepository.Verify(
            repo => repo.AddAsync(
                It.Is<University>(u =>
                    u.Name == name && 
                    u.Country == country 
                ),
                It.IsAny<CancellationToken>() 
            ),
            Times.Once() 
        );

        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDataIncorrect()
    {
        // ---- ARRANGE ----
        var command = new CreateUniversityCommand("", "Country", "Voivo", "City", "Address");
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<University>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException()); 

        // ---- ACT ----
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<ArgumentException>();

        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never()
        );
    }
}
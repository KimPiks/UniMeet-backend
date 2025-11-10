using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.DeleteAllowedEmailDomain;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;

public class DeleteAllowedEmailDomainCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly DeleteAllowedEmailDomainCommandHandler _handler;

    public DeleteAllowedEmailDomainCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new DeleteAllowedEmailDomainCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveDomainAndSaveChanges_WhenDomainExists()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddAllowedEmailDomain("to-delete.com", 1);

        var domainToRemove = fakeUniversity.AllowedEmailDomains.First();
        var idProperty = typeof(AllowedEmailDomain).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (idProperty == null)
            throw new InvalidOperationException("Property 'Id' not found on AllowedEmailDomain.");
        idProperty.SetValue(domainToRemove, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new DeleteAllowedEmailDomainCommand(1, 5);

        // ---- ACT ----
        await _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        fakeUniversity.AllowedEmailDomains.Should().BeEmpty();
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
            
        var command = new DeleteAllowedEmailDomainCommand(1, 99);

        // ---- ACT ----
        Func<Task> act = () => _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Allowed email domain not found");
    }
}
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Features.Departments.Commands.AddDepartment;

public class AddDepartmentCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly AddDepartmentCommandHandler _handler;

    public AddDepartmentCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new AddDepartmentCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddDepartmentAndSaveChanges_WhenUniversityExists()
    {
        // ---- ARRANGE ----
        var departmentName = "IT Faculty";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new AddDepartmentCommand(1, departmentName);

        // ---- ACT ----
        await _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        fakeUniversity.Departments.Should().HaveCount(1);
        fakeUniversity.Departments.First().Name.Should().Be(departmentName);

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
            
        var command = new AddDepartmentCommand(99, "Test Dept");

        // ---- ACT ----
        Func<Task> act = () => _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("University not found");
    }
}
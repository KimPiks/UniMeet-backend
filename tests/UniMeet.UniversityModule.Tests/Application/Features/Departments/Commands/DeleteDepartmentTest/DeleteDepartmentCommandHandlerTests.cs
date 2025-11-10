using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly DeleteDepartmentCommandHandler _handler;

    public DeleteDepartmentCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new DeleteDepartmentCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveDepartmentAndSaveChanges_WhenDepartmentExists()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("To Be Deleted", 1);

        var departmentToRemove = fakeUniversity.Departments.First();

        var idProp = typeof(Department).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (idProp == null)
            throw new InvalidOperationException("Id property not found on Department");
        idProp.SetValue(departmentToRemove, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new DeleteDepartmentCommand(1, 5);

        // ---- ACT ----
        await _handler.HandleAsync(command, CancellationToken.None); 

        // ---- ASSERT ----
        fakeUniversity.Departments.Should().BeEmpty();
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDepartmentNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new DeleteDepartmentCommand(1, 99);

        // ---- ACT ----
        Func<Task> act = () => _handler.HandleAsync(command, CancellationToken.None); 

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Department not found");
    }
}
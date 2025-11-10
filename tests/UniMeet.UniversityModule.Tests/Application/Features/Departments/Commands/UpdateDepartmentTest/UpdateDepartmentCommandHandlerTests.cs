using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UniMeet.UniversityModule.Application.Departments.UpdateDepartment;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Universities;

public class UpdateDepartmentCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly UpdateDepartmentCommandHandler _handler;

    public UpdateDepartmentCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new UpdateDepartmentCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldRenameDepartmentAndSaveChanges_WhenDataIsProvided()
    {
        // ---- ARRANGE ----
        var oldName = "Old Name";
        var newName = "New Faculty Name";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment(oldName, 1);

        var departmentToUpdate = fakeUniversity.Departments.First();
        var idProperty = typeof(Department).GetProperty("Id");
        if (idProperty == null)
            throw new InvalidOperationException("Property 'Id' not found on Department type.");
        idProperty.SetValue(departmentToUpdate, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new UpdateDepartmentCommand(1, 5, newName);

        // ---- ACT ----
        await _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        departmentToUpdate.Name.Should().Be(newName);
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
            
        var command = new UpdateDepartmentCommand(1, 99, "New Name");

        // ---- ACT ----
        Func<Task> act = () => _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Department not found");
    }
}
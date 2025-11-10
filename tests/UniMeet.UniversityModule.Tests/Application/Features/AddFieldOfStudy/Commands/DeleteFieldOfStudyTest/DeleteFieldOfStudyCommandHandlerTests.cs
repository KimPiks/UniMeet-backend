using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Application.FieldsOfStudy.DeleteFieldOfStudy;
using UniMeet.UniversityModule.Domain.Universities;

public class DeleteFieldOfStudyCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly DeleteFieldOfStudyCommandHandler _handler;

    public DeleteFieldOfStudyCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new DeleteFieldOfStudyCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveFieldOfStudyAndSaveChanges_WhenAllExist()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);
        var department = fakeUniversity.Departments.First();
        fakeUniversity.AddFieldOfStudyToDepartment(department.Name, "To Be Deleted");
        var fieldOfStudy = department.FieldsOfStudy.First();

        var idProp = typeof(Department).GetProperty("Id");
        if (idProp == null)
        {
            throw new InvalidOperationException("Department.Id property not found.");
        }
        idProp.SetValue(department, 5);


        var idProperty = typeof(FieldOfStudy).GetProperty("Id");
        if (idProperty == null)
        {
            throw new InvalidOperationException("FieldOfStudy.Id property not found.");
        }
        idProperty.SetValue(fieldOfStudy, 10);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new DeleteFieldOfStudyCommand(1, 5, 10);

        // ---- ACT ----
        await _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        department.FieldsOfStudy.Should().BeEmpty();
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenFieldOfStudyNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);
        var department = fakeUniversity.Departments.First();
        var idProp = typeof(Department).GetProperty("Id");
        if (idProp == null)
        {
            throw new InvalidOperationException("Department.Id property not found.");
        }
        idProp.SetValue(department, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var command = new DeleteFieldOfStudyCommand(1, 5, 99);

        // ---- ACT ----
        Func<Task> act = () => _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Field of study not found");
    }
}
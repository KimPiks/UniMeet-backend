using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Application.FieldsOfStudy.AddFieldOfStudy;
using UniMeet.UniversityModule.Domain.Universities;

public class AddFieldOfStudyCommandHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly AddFieldOfStudyCommandHandler _handler;

    public AddFieldOfStudyCommandHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new AddFieldOfStudyCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddFieldOfStudyAndSaveChanges_WhenAllExists()
    {
        // ---- ARRANGE ----
        var fieldName = "Computer Science";
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
            
        var command = new AddFieldOfStudyCommand(1, 5, fieldName);

        // ---- ACT ----
        await _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        department.FieldsOfStudy.Should().HaveCount(1);
        department.FieldsOfStudy.First().Name.Should().Be(fieldName);
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
            
        var command = new AddFieldOfStudyCommand(1, 99, "Test Field");

        // ---- ACT ----
        Func<Task> act = () => _handler.HandleAsync(command, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Department not found");
    }
}
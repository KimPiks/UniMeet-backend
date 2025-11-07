using Moq;
using Xunit;
using System.Threading;
using FluentAssertions;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Tests.Application;

public partial class UniversityServiceTests
{
    // --- Tests for AddFieldOfStudyAsync ---

    [Fact]
    public async Task AddFieldOfStudyAsync_ShouldAddFieldOfStudyAndSaveChanges_WhenAllExists()
    {
        // ---- ARRANGE ----
        var fieldName = "Computer Science";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);

        var department = fakeUniversity.Departments.First();
        typeof(Department).GetProperty("Id").SetValue(department, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.AddFieldOfStudyAsync(1, 5, fieldName);

        // ---- ASSERT ----
        department.FieldsOfStudy.Should().HaveCount(1);
        department.FieldsOfStudy.First().Name.Should().Be(fieldName);
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task AddFieldOfStudyAsync_ShouldThrowException_WhenDepartmentNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        Func<Task> act = () => _service.AddFieldOfStudyAsync(1, 99, "Test Field");

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Department not found");
    }

    // --- Tests for GetFieldsOfStudyByDepartmentIdAsync ---

    [Fact]
    public async Task GetFieldsOfStudyByDepartmentIdAsync_ShouldReturnMappedDtos_WhenFieldsOfStudyExist()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);
        var department = fakeUniversity.Departments.First();
        fakeUniversity.AddFieldOfStudyToDepartment(department.Name, "Computer Science");
        
        typeof(Department).GetProperty("Id").SetValue(department, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        var result = await _service.GetFieldsOfStudyByDepartmentIdAsync(1, 5);
        var resultList = result.ToList();

        // ---- ASSERT ----
        resultList.Should().NotBeNull();
        resultList.Should().HaveCount(1);
        resultList.First().Name.Should().Be("Computer Science");
    }

    [Fact]
    public async Task GetFieldsOfStudyByDepartmentIdAsync_ShouldThrowException_WhenDepartmentNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        Func<Task> act = () => _service.GetFieldsOfStudyByDepartmentIdAsync(1, 99);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Department not found");
    }

    // --- Tests for DeleteFieldOfStudyAsync ---

    [Fact]
    public async Task DeleteFieldOfStudyAsync_ShouldRemoveFieldOfStudyAndSaveChanges_WhenAllExist()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);
        var department = fakeUniversity.Departments.First();
        fakeUniversity.AddFieldOfStudyToDepartment(department.Name, "To Be Deleted");
        var fieldOfStudy = department.FieldsOfStudy.First();

        typeof(Department).GetProperty("Id").SetValue(department, 5);
        typeof(FieldOfStudy).GetProperty("Id").SetValue(fieldOfStudy, 10);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.DeleteFieldOfStudyAsync(1, 5, 10);

        // ---- ASSERT ----
        department.FieldsOfStudy.Should().BeEmpty();
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task DeleteFieldOfStudyAsync_ShouldThrowException_WhenFieldOfStudyNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);
        var department = fakeUniversity.Departments.First();
        typeof(Department).GetProperty("Id").SetValue(department, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        Func<Task> act = () => _service.DeleteFieldOfStudyAsync(1, 5, 99);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Field of study not found");
    }

    // --- Tests for UpdateFieldOfStudyAsync ---

    [Fact]
    public async Task UpdateFieldOfStudyAsync_ShouldUpdateFieldOfStudyAndSaveChanges_WhenDataIsProvided()
    {
        // ---- ARRANGE ----
        var oldName = "Old Name";
        var newName = "New Name";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);
        var department = fakeUniversity.Departments.First();
        fakeUniversity.AddFieldOfStudyToDepartment(department.Name, oldName);
        var fieldOfStudy = department.FieldsOfStudy.First();

        typeof(Department).GetProperty("Id").SetValue(department, 5);
        typeof(FieldOfStudy).GetProperty("Id").SetValue(fieldOfStudy, 10);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.UpdateFieldOfStudyAsync(1, 5, 10, newName);

        // ---- ASSERT ----
        fieldOfStudy.Name.Should().Be(newName);
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task UpdateFieldOfStudyAsync_ShouldThrowException_WhenFieldOfStudyNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);
        var department = fakeUniversity.Departments.First();
        typeof(Department).GetProperty("Id").SetValue(department, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        Func<Task> act = () => _service.UpdateFieldOfStudyAsync(1, 5, 99, "New Name");

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Field of study not found");
    }
}
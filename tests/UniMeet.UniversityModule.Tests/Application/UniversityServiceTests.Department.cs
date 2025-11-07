using Moq;
using Xunit;
using System.Threading;
using FluentAssertions;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;

namespace UniMeet.UniversityModule.Tests.Application;


public partial class UniversityServiceTests
{
    [Fact]
    public async Task AddDepartmentAsync_ShouldAddDepartmentAndSaveChanges_WhenUniversityExists()
    {
        // ---- ARRANGE ----
        var departmentName = "IT Faculty";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.AddDepartmentAsync(1, departmentName);

        // ---- ASSERT ----
        fakeUniversity.Departments.Should().HaveCount(1);
        fakeUniversity.Departments.First().Name.Should().Be(departmentName);

        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task AddDepartmentAsync_ShouldThrowException_WhenUniversityNotFound()
    {
        // ---- ARRANGE ----
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((University?)null);

        // ---- ACT ----
        Func<Task> act = () => _service.AddDepartmentAsync(99, "Test Dept");

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("University not found");
    }

    [Fact]
    public async Task GetDepartmentsByUniversityIdAsync_ShouldReturnMappedDtos_WhenDepartmentsExist()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);
        fakeUniversity.AddDepartment("Management Faculty", 1);
        fakeUniversity.AddFieldOfStudyToDepartment("IT Faculty", "Computer Science");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        var result = await _service.GetDepartmentsByUniversityIdAsync(1);
        var resultList = result.ToList();

        // ---- ASSERT ----
        resultList.Should().NotBeNull();
        resultList.Should().HaveCount(2);
        resultList.First().Name.Should().Be("IT Faculty");
        resultList.First().FieldsOfStudy.Should().HaveCount(1);
        resultList.First().FieldsOfStudy.First().Name.Should().Be("Computer Science");
    }

    [Fact]
    public async Task GetDepartmentsByUniversityIdAsync_ShouldReturnEmptyList_WhenNoDepartmentsExist()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        var result = await _service.GetDepartmentsByUniversityIdAsync(1);

        // ---- ASSERT ----
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }


    [Fact]
    public async Task DeleteDepartmentAsync_ShouldRemoveDepartmentAndSaveChanges_WhenDepartmentExists()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("To Be Deleted", 1);

        var departmentToRemove = fakeUniversity.Departments.First();
        typeof(Department).GetProperty("Id").SetValue(departmentToRemove, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.DeleteDepartmentAsync(1, 5); 

        // ---- ASSERT ----
        fakeUniversity.Departments.Should().BeEmpty();
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ShouldThrowException_WhenDepartmentNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        Func<Task> act = () => _service.DeleteDepartmentAsync(1, 99); 

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Department not found");
    }


    [Fact]
    public async Task UpdateDepartmentAsync_ShouldRenameDepartmentAndSaveChanges_WhenDataIsProvided()
    {
        // ---- ARRANGE ----
        var oldName = "Old Name";
        var newName = "New Faculty Name";
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment(oldName, 1);

        var departmentToUpdate = fakeUniversity.Departments.First();
        typeof(Department).GetProperty("Id").SetValue(departmentToUpdate, 5);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.UpdateDepartmentAsync(1, 5, newName);

        // ---- ASSERT ----
        departmentToUpdate.Name.Should().Be(newName);
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task UpdateDepartmentAsync_ShouldThrowException_WhenDepartmentNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        Func<Task> act = () => _service.UpdateDepartmentAsync(1, 99, "New Name");

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Department not found");
    }
}
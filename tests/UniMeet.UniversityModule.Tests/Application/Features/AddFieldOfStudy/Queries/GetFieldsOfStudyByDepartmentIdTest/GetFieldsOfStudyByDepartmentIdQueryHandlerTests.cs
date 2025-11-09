using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Features.FieldsOfStudy.Queries.GetFieldsOfStudyByDepartmentId;
using UniMeet.UniversityModule.Application.DTOs; // Wymagane dla DTO

public class GetFieldsOfStudyByDepartmentIdQueryHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly GetFieldsOfStudyByDepartmentIdQueryHandler _handler;

    public GetFieldsOfStudyByDepartmentIdQueryHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new GetFieldsOfStudyByDepartmentIdQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedDtos_WhenFieldsOfStudyExist()
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
            
        var query = new GetFieldsOfStudyByDepartmentIdQuery(1, 5);

        // ---- ACT ----
        var result = await _handler.Handle(query, CancellationToken.None);
        var resultList = result.ToList();

        // ---- ASSERT ----
        resultList.Should().NotBeNull();
        resultList.Should().HaveCount(1);
        resultList.First().Name.Should().Be("Computer Science");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenDepartmentNotFound()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var query = new GetFieldsOfStudyByDepartmentIdQuery(1, 99);

        // ---- ACT ----
        Func<Task> act = () => _handler.Handle(query, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("Department not found");
    }
}
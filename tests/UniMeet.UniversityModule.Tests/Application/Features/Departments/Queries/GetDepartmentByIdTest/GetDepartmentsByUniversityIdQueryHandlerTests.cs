using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using UniMeet.UniversityModule.Application.Departments.GetDepartmentsByUniversityId;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Mappers; 

public class GetDepartmentsByUniversityIdQueryHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly GetDepartmentsByUniversityIdQueryHandler _handler;

    public GetDepartmentsByUniversityIdQueryHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new GetDepartmentsByUniversityIdQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedDtos_WhenDepartmentsExist()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddDepartment("IT Faculty", 1);
        fakeUniversity.AddDepartment("Management Faculty", 1);
        fakeUniversity.AddFieldOfStudyToDepartment("IT Faculty", "Computer Science");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var query = new GetDepartmentsByUniversityIdQuery(1);

        // ---- ACT ----
        var result = await _handler.HandleAsync(query, CancellationToken.None);
        var resultList = result.ToList();

        // ---- ASSERT ----
        resultList.Should().NotBeNull();
        resultList.Should().HaveCount(2);
        resultList.First().Name.Should().Be("IT Faculty");
        resultList.First().FieldsOfStudy.Should().HaveCount(1);
        resultList.First().FieldsOfStudy.First().Name.Should().Be("Computer Science");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoDepartmentsExist()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var query = new GetDepartmentsByUniversityIdQuery(1);

        // ---- ACT ----
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // ---- ASSERT ----
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Universities.Queries.GetUniversityById;
using UniMeet.UniversityModule.Application.Mappers; 

public class GetUniversityByIdQueryHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly GetUniversityByIdQueryHandler _handler; 

    public GetUniversityByIdQueryHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new GetUniversityByIdQueryHandler(_mockRepository.Object); 
    }

    [Fact]
    public async Task Handle_ShouldReturnUniversityDto_WhenUniversityExists()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Name", "Test Country", "Test Voivo", "Test City", "Test Address");
        fakeUniversity.AddAllowedEmailDomain("test.edu", 1);
        fakeUniversity.AddDepartment("Wydział Informatyki", 1);
        fakeUniversity.AddFieldOfStudyToDepartment("Wydział Informatyki", "KASK");
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        var query = new GetUniversityByIdQuery(1);

        //---- ACT -----
        var result = await _handler.Handle(query, CancellationToken.None);

        // ---- ASSERT ----
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Name");
        result.Id.Should().Be(fakeUniversity.Id);
        result.Country.Should().Be("Test Country");
        result.Voivodeship.Should().Be("Test Voivo");
        result.City.Should().Be("Test City");
        result.Address.Should().Be("Test Address");

        //Domain test
        result.Departments.Should().NotBeNull();
        result.Departments.Should().HaveCount(1);
        result.Departments.First().Name.Should().Be("Wydział Informatyki");

        result.Departments.First().FieldsOfStudy.Should().NotBeNull();
        result.Departments.First().FieldsOfStudy.Should().HaveCount(1);
        result.Departments.First().FieldsOfStudy.First().Name.Should().Be("KASK");
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenUniversityDoesntExists()
    {
        // ---- ARRANGE ----
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((University?)null);
            
        var query = new GetUniversityByIdQuery(1);

        // ---- ACT ---- 
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // ---- ASSERT ----
        result.Should().BeNull();
    }
}
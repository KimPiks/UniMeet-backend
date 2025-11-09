using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic; 
using System.Linq;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Universities.Queries.GetAllUniversities; 
using UniMeet.UniversityModule.Application.Mappers; 

public class GetAllUniversitiesQueryHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly GetAllUniversitiesQueryHandler _handler;

    public GetAllUniversitiesQueryHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new GetAllUniversitiesQueryHandler(_mockRepository.Object); 
    }

    [Fact] 
    public async Task Handle_ShouldReturnMappedDtos_WhenUniversitiesExist()
    {
        // ---- ARRANGE ----
        var fakeUni1 = new University("Test uni 1", "Polska", "Pomorskie", "Gdańsk", "Chojnicka 13");
        var fakeUni2 = new University("Test uni 2", "Maroko", "Kujawsko-Pomorskie", "Gdańsk", "Bałdowska 13");

        fakeUni2.AddAllowedEmailDomain("uni2.edu", 2);
        fakeUni2.AddDepartment("Wydział Elektroniki", 2); 

        var fakeUniversities = new List<University> { fakeUni1, fakeUni2 };

        _mockRepository
            .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversities);
            

        var query = new GetAllUniversitiesQuery();

        //---- ACT -----
        var result = await _handler.Handle(query, CancellationToken.None);
        var resultList = result.ToList();

        // ---- ASSERT ----
        resultList.Should().NotBeNull();
        resultList.Should().HaveCount(2);

        resultList[1].Name.Should().Be("Test uni 2");
        resultList[1].Departments.Should().HaveCount(1);
        resultList[1].Departments.First().Name.Should().Be("Wydział Elektroniki"); 
        resultList[1].AllowedEmailDomains.Should().HaveCount(1);
        resultList[1].AllowedEmailDomains.First().Domain.Should().Be("uni2.edu");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenUniversitiesDoesntExist()
    {
        // ---- ARRANGE ----
        var emptyList = new List<University>();

        _mockRepository
            .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);
            
        var query = new GetAllUniversitiesQuery();

        // ---- ACT ----
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // ---- ASSERT ---- 
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
using Moq;
using Xunit;
using System.Threading;
using FluentAssertions;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Application.Services;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Interfaces;

namespace UniMeet.UniversityModule.Tests.Application;

public class UniversityServiceTests
{
    private readonly Mock<IUniversityRepository> _mockRepository; 
    private readonly IUniversityService _service;

    public UniversityServiceTests()
    {
        _mockRepository = new Mock<IUniversityRepository>(); 
        _service = new UniversityService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetUniversityByIdAsync_ShouldReturnUniversityDto_WhenUniversityExists()
    {
        var fakeUniversity = new University("Test Name", "Test Country", "Test Voivo", "Test City", "Test Address");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var result = await _service.GetUniversityByIdAsync(1);


        result.Should().NotBeNull();
        result.Name.Should().Be("Test Name");
        result.Id.Should().Be(fakeUniversity.Id);
        result.Country.Should().Be("Test Country");
        result.Voivodeship.Should().Be("Test Voivo");
        result.City.Should().Be("Test City");
        result.Address.Should().Be("Test Address");
    }
}
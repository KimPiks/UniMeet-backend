using Moq;
using Xunit;
using System.Threading;
using FluentAssertions;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Application.Services;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace UniMeet.UniversityModule.Tests.Application;

public partial class UniversityServiceTests
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
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Name", "Test Country", "Test Voivo", "Test City", "Test Address");
        fakeUniversity.AddAllowedEmailDomain("test.edu", 1);
        fakeUniversity.AddDepartment("Wydział Informatyki", 1);
        fakeUniversity.AddFieldOfStudyToDepartment("Wydział Informatyki", "KASK");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        //---- ACT -----
        var result = await _service.GetUniversityByIdAsync(1);

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
    public async Task Get_UniversityByIdAsync_ShouldReturnNull_WhenUniversityDoesntExists()
    {
        // ---- arrange ----
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((University?)null);

        // ---- ACT ---- 
        var result = await _service.GetUniversityByIdAsync(1);
        // ---- ASSERT ----
        result.Should().BeNull();
    }

    public async Task GetAllUniversitiesAsync_ShouldReturnMappedDtos_WhenUniversitiesExist()
    {

        // ---- Arange ----

        var fakeUni1 = new University("Test uni 1", "Polska", "Pomorskie", "Gdańsk", "Chojnicka 13");
        var fakeUni2 = new University("Test uni 2", "Maroko", "Kujawsko-Pomorskie", "Gdańsk", "Bałdowska 13");

        fakeUni2.AddAllowedEmailDomain("uni2.edu", 2);
        fakeUni2.AddDepartment("Wydział Elektroniki", 2);

        var fakeUniversities = new List<University> { fakeUni1, fakeUni2 };

        _mockRepository
        .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(fakeUniversities);

        // ---- ACT ----
        var result = await _service.GetAllUniversitiesAsync();
        var resultList = result.ToList();


        // ---- Assert ----
        resultList.Should().NotBeNull();
        resultList.Should().HaveCount(2);

        resultList[1].Name.Should().Be("Test Uni 2");
        resultList[1].Departments.Should().HaveCount(1);
        resultList[1].Departments.First().Name.Should().Be("Wydział IT");
        resultList[1].AllowedEmailDomains.Should().HaveCount(1);
        resultList[1].AllowedEmailDomains.First().Domain.Should().Be("uni2.edu");
    }

    [Fact]
    public async Task GetAllUniversitiesAsync_ShouldReturnEmptyList_WhenUniversitiesDoesntExist()
    {
        // ---- Arrange ----
        var emptyList = new List<University>();

        _mockRepository
        .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(emptyList);

        // ---- Act ----
        var result = await _service.GetAllUniversitiesAsync();
        // ---- Assert ---- 
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateUniversityAsync_ShouldAddUniversityAndCallSaveChangesAsync_WhenDataIsCorrect()
    {
        // ---- ARRANGE ----
        var name = "Test University";
        var country = "Test Country";

        // ---- ACT ----
        await _service.CreateUniversityAsync(name, country, "Test Voivo", "Test City", "Test Address");

        // ---- ASSERT ----
        _mockRepository.Verify(
            repo => repo.AddAsync(
                It.Is<University>(u =>
                    u.Name == name && 
                    u.Country == country 
                ),
                It.IsAny<CancellationToken>() 
            ),
            Times.Once() 
        );

        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task CreateUniversityAsync_ShouldThrowException_WhenDataIncorrect()
    {
        // ---- ARRANGE ----
        // ---- ACT ----
        Func<Task> act = () => _service.CreateUniversityAsync(null, "Country", "Voivo", "City", "Address");

        // ASSERT
        await act.Should().ThrowAsync<ArgumentNullException>();

        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never()
        );
    }

    [Fact]
    public async Task UpdateUniversityAsync_ShouldUpdateUniversityAndCallSaveChangesAsync_WhenDataIsProvided()
    {
        // ---- ARRANGE ----
        var newName = "New uni name";
        var newCountry = "New country";
        
        var fakeUniversity = new University("Old name", "Old country", "Old Voivo", "Old city", "Old addres");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.UpdateUniversityAsync(1, newName, newCountry, null, null, null);

        // ---- ASSERT ----
        
        fakeUniversity.Name.Should().Be(newName);
        fakeUniversity.Country.Should().Be(newCountry);

        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task UpdateUniversityAsync_ShouldThrowArgumentException_WhenUniversityNotFound()
    {
        // ---- ARRANGE ----
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((University?)null);

        // ---- ACT ----
        Func<Task> act = () => _service.UpdateUniversityAsync(99, "New name", null, null, null, null);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("University not found"); 
                 
        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never()
        );
    }

    [Fact]
    public async Task UpdateUniversityAsync_ShouldNotUpdateUniversityButCallSaveChangesAsync_WhenDataIsNull()
    {
        // ---- ARRANGE ----
        var oldName = "Old Name";
        var fakeUniversity = new University(oldName, "Old country", "Old Voivo", "Old City", "Old addres");

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);

        // ---- ACT ----
        await _service.UpdateUniversityAsync(1, null, null, null, null, null);

        // ---- ASSERT ----
        fakeUniversity.Name.Should().Be(oldName);

        _mockRepository.Verify(
            repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }
    

}
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;
using UniMeet.UniversityModule.Domain.Aggregates.UniversityAggregate;
using UniMeet.UniversityModule.Domain.Repositories;
using UniMeet.UniversityModule.Application.Mappers;

public class GetAllowedEmailDomainsByUniversityIdQueryHandlerTests
{
    private readonly Mock<IUniversityRepository> _mockRepository;
    private readonly GetAllowedEmailDomainsByUniversityIdQueryHandler _handler;

    public GetAllowedEmailDomainsByUniversityIdQueryHandlerTests()
    {
        _mockRepository = new Mock<IUniversityRepository>();
        _handler = new GetAllowedEmailDomainsByUniversityIdQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedDtos_WhenDomainsExist()
    {
        // ---- ARRANGE ----
        var fakeUniversity = new University("Test Uni", "Country", "Voivo", "City", "Address");
        fakeUniversity.AddAllowedEmailDomain("test1.edu", 1);
        fakeUniversity.AddAllowedEmailDomain("test2.edu", 1);

        _mockRepository
            .Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeUniversity);
            
        var query = new GetAllowedEmailDomainsByUniversityIdQuery(1);

        // ---- ACT ----
        var result = await _handler.HandleAsync(query, CancellationToken.None);
        var resultList = result.ToList();

        // ---- ASSERT ----
        resultList.Should().NotBeNull();
        resultList.Should().HaveCount(2);
        resultList.First().Domain.Should().Be("test1.edu");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUniversityNotFound()
    {
        // ---- ARRANGE ----
        _mockRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((University?)null);
            
        var query = new GetAllowedEmailDomainsByUniversityIdQuery(99);

        // ---- ACT ----
        Func<Task> act = () => _handler.HandleAsync(query, CancellationToken.None);

        // ---- ASSERT ----
        await act.Should()
                 .ThrowAsync<ArgumentException>()
                 .WithMessage("University not found");
    }
}
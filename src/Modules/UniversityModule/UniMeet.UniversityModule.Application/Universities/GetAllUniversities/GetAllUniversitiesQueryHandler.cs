using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.Universities.GetAllUniversities;

public class GetAllUniversitiesQueryHandler(IUniversityRepository universityRepository)
    : IRequestHandler<GetAllUniversitiesQuery, IEnumerable<UniversityDto>>
{
    public async Task<IEnumerable<UniversityDto>> HandleAsync(GetAllUniversitiesQuery request, CancellationToken cancellationToken)
        {
            var universities = await universityRepository.GetAllAsync(cancellationToken);
            return universities.Select(university => university.ToDto()).ToList();
        }
    }
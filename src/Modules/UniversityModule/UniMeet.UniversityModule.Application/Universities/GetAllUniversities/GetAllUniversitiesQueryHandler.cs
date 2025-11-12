using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.GetAllUniversities;

public class GetAllUniversitiesQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetAllUniversitiesQuery, IEnumerable<UniversityDto>>
{
    public async Task<IEnumerable<UniversityDto>> HandleAsync(GetAllUniversitiesQuery request, CancellationToken cancellationToken)
        {
            request.Validate();
            
            var universities = await universityRepository.GetAllAsync(request.Offset, request.Limit, cancellationToken);
            return universities.Select(university => university.ToDto()).ToList();
        }
    }
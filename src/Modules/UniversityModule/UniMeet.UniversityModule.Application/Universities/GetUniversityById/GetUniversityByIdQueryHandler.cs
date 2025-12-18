using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.GetUniversityById;

public class GetUniversityByIdQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetUniversityByIdQuery, UniversityDto?>
{
    public async Task<UniversityDto?> HandleAsync(GetUniversityByIdQuery request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);

        if (university == null)
            return null;
        
        return university.ToDto();
    }
}
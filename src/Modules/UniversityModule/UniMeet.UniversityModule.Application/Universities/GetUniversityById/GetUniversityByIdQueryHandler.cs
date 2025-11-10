using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.Universities.GetUniversityById;


public class GetUniversityByIdQueryHandler(IUniversityRepository universityRepository)
    : IRequestHandler<GetUniversityByIdQuery, UniversityDto?>
{
    public async Task<UniversityDto?> HandleAsync(GetUniversityByIdQuery request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);

        if (university == null)
        {
            return null;
        }

        return university.ToDto();
    }
}
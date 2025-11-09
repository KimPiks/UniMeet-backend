using MediatR;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers;
using UniMeet.UniversityModule.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace UniMeet.UniversityModule.Application.Universities.Queries.GetUniversityById;


public class GetUniversityByIdQueryHandler : IRequestHandler<GetUniversityByIdQuery, UniversityDto?>
{
    private readonly IUniversityRepository _universityRepository;

    public GetUniversityByIdQueryHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<UniversityDto?> Handle(GetUniversityByIdQuery request, CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);

        if (university == null)
        {
            return null;
        }

        return university.ToDto();
    }
}
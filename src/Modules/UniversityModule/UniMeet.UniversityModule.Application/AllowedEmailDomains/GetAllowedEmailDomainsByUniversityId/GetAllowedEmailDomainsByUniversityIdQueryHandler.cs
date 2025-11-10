using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;

public class GetAllowedEmailDomainsByUniversityIdQueryHandler(IUniversityRepository universityRepository)
    : IRequestHandler<GetAllowedEmailDomainsByUniversityIdQuery, IEnumerable<AllowedEmailDomainDto>>
{
    public async Task<IEnumerable<AllowedEmailDomainDto>> HandleAsync(GetAllowedEmailDomainsByUniversityIdQuery request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }

        return university.AllowedEmailDomains.Select(allowedDomain => allowedDomain.ToDto()).ToList();
    }
}
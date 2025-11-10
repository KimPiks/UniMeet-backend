using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;

public class GetAllowedEmailDomainsByUniversityIdQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetAllowedEmailDomainsByUniversityIdQuery, IEnumerable<AllowedEmailDomainDto>>
{
    public async Task<IEnumerable<AllowedEmailDomainDto>> HandleAsync(GetAllowedEmailDomainsByUniversityIdQuery request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
            throw new ArgumentException("University not found");

        return university.AllowedEmailDomains.Select(allowedDomain => allowedDomain.ToDto()).ToList();
    }
}
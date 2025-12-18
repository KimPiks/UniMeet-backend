using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.Universities.GetByAllowedDomain;

public class GetByAllowedDomainQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetByAllowedDomainQuery, UniversityDto?>
{
    public async Task<UniversityDto?> HandleAsync(GetByAllowedDomainQuery request, CancellationToken cancellationToken = default)
    {
        request.Validate();
        
        var university = await universityRepository.GetByAllowedEmailAsync(request.Domain, cancellationToken);
        return university?.ToDto();
    }
}
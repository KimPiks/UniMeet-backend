using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainById;

public class GetAllowedEmailDomainByIdQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetAllowedEmailDomainByIdQuery, AllowedEmailDomainDto?>
{
    public async Task<AllowedEmailDomainDto?> HandleAsync(GetAllowedEmailDomainByIdQuery request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        var domain = university.AllowedEmailDomains.FirstOrDefault(d => d.Id == request.DomainId);
        
        return domain?.ToDto();
    }
}
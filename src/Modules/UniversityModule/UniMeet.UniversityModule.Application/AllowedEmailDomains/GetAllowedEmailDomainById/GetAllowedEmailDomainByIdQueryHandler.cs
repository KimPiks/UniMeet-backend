using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainById;

public class GetAllowedEmailDomainByIdQueryHandler(IUniversityRepository universityRepository)
    : IRequestHandler<GetAllowedEmailDomainByIdQuery, AllowedEmailDomainDto?>
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
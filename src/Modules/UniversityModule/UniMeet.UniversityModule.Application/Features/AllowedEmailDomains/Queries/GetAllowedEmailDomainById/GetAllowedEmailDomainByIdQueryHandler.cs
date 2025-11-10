using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers;
using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Queries.GetAllowedEmailDomainById;

public class GetAllowedEmailDomainByIdQueryHandler : IRequestHandler<GetAllowedEmailDomainByIdQuery, AllowedEmailDomainDto?>
{
    private readonly IUniversityRepository _universityRepository;

    public GetAllowedEmailDomainByIdQueryHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<AllowedEmailDomainDto?> HandleAsync(GetAllowedEmailDomainByIdQuery request, CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        var domain = university.AllowedEmailDomains.FirstOrDefault(d => d.Id == request.DomainId);
        
        return domain?.ToDto();
    }
}
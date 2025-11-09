using MediatR;
using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers; // Upewnij się, że masz ToDto() dla AllowedEmailDomain
using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Queries.GetAllowedEmailDomainsByUniversityId;

public class GetAllowedEmailDomainsByUniversityIdQueryHandler : IRequestHandler<GetAllowedEmailDomainsByUniversityIdQuery, IEnumerable<AllowedEmailDomainDto>>
{
    private readonly IUniversityRepository _universityRepository;

    public GetAllowedEmailDomainsByUniversityIdQueryHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<IEnumerable<AllowedEmailDomainDto>> Handle(GetAllowedEmailDomainsByUniversityIdQuery request, CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }

        return university.AllowedEmailDomains.Select(allowedDomain => allowedDomain.ToDto()).ToList();
    }
}
using MediatR;
using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.AddAllowedEmailDomain;

public class AddAllowedEmailDomainCommandHandler : IRequestHandler<AddAllowedEmailDomainCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public AddAllowedEmailDomainCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task Handle(AddAllowedEmailDomainCommand request, CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        university.AddAllowedEmailDomain(request.Domain, request.UniversityId);
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}
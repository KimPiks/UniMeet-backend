using MediatR;
using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.UpdateAllowedEmailDomain;

public class UpdateAllowedEmailDomainCommandHandler : IRequestHandler<UpdateAllowedEmailDomainCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public UpdateAllowedEmailDomainCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task Handle(UpdateAllowedEmailDomainCommand request, CancellationToken cancellationToken)
    {
        var university = await _universityRepository.GetByIdAsync(request.UniversityId);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }

        var domain = university.AllowedEmailDomains.FirstOrDefault(d => d.Id == request.DomainId);
        if (domain == null)
        {
            throw new ArgumentException("Allowed email domain not found");
        }

        if (!string.IsNullOrEmpty(request.NewDomain))
        {
            university.ChangeAllowedEmailDomain(domain.Domain, request.NewDomain);
        }
        
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}
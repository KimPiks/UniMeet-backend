using UniMeet.UniversityModule.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Features.AllowedEmailDomains.Commands.DeleteAllowedEmailDomain;

public class DeleteAllowedEmailDomainCommandHandler : IRequestHandler<DeleteAllowedEmailDomainCommand>
{
    private readonly IUniversityRepository _universityRepository;

    public DeleteAllowedEmailDomainCommandHandler(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task HandleAsync(DeleteAllowedEmailDomainCommand request, CancellationToken cancellationToken)
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

        university.RemoveAllowedEmailDomain(domain.Domain);
        await _universityRepository.SaveChangesAsync(cancellationToken);
    }
}
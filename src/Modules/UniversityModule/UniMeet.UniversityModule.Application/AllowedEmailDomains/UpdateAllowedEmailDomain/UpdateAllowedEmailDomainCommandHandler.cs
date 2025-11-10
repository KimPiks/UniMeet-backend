using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Repositories;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.UpdateAllowedEmailDomain;

public class UpdateAllowedEmailDomainCommandHandler(IUniversityRepository universityRepository)
    : IRequestHandler<UpdateAllowedEmailDomainCommand>
{
    public async Task HandleAsync(UpdateAllowedEmailDomainCommand request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
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
        
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}
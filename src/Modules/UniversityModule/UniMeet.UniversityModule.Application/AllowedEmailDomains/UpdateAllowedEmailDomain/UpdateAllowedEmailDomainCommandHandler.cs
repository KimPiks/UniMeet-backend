using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.UpdateAllowedEmailDomain;

public class UpdateAllowedEmailDomainCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<UpdateAllowedEmailDomainCommand>
{
    public async Task HandleAsync(UpdateAllowedEmailDomainCommand request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByAllowedDomainIdAsync(request.DomainId, cancellationToken);
        if (university == null)
            throw new ArgumentException("University not found");

        var domain = university.AllowedEmailDomains.FirstOrDefault(d => d.Id == request.DomainId);
        if (domain == null)
            throw new ArgumentException("Allowed email domain not found");

        university.ChangeAllowedEmailDomain(request.DomainId, request.NewDomain);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}
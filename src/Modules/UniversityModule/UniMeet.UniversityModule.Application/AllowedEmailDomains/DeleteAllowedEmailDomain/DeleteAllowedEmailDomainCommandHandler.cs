using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.DeleteAllowedEmailDomain;

public class DeleteAllowedEmailDomainCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<DeleteAllowedEmailDomainCommand>
{
    public async Task HandleAsync(DeleteAllowedEmailDomainCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var university = await universityRepository.GetByAllowedDomainIdAsync(request.DomainId, cancellationToken);
        if (university == null)
            throw new KeyNotFoundException("University not found");

        var domain = university.AllowedEmailDomains.FirstOrDefault(d => d.Id == request.DomainId);
        if (domain == null)
            throw new KeyNotFoundException("Allowed email domain not found");

        university.RemoveAllowedEmailDomain(request.DomainId);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}
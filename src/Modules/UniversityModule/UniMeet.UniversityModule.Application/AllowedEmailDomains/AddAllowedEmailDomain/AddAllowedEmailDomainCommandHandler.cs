using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.AddAllowedEmailDomain;

public class AddAllowedEmailDomainCommandHandler(IUniversityRepository universityRepository)
    : ICommandHandler<AddAllowedEmailDomainCommand>
{
    public async Task HandleAsync(AddAllowedEmailDomainCommand request, CancellationToken cancellationToken)
    {
        var university = await universityRepository.GetByIdAsync(request.UniversityId, cancellationToken);
        if (university == null)
        {
            throw new ArgumentException("University not found");
        }
        
        university.AddAllowedEmailDomain(request.Domain, request.UniversityId);
        await universityRepository.SaveChangesAsync(cancellationToken);
    }
}
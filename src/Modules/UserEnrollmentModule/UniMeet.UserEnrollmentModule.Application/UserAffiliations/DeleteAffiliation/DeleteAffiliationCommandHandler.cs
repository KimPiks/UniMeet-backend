using UniMeet.Shared.Abstractions;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation.Exceptions;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.DeleteAffiliation;

public class DeleteAffiliationCommandHandler(IUserAffiliationRepository repository) : ICommandHandler<DeleteAffiliationCommand>
{
    public async Task HandleAsync(DeleteAffiliationCommand request, CancellationToken cancellationToken = default)
    {
        var affiliation = await repository.GetByIdAsync(request.AffiliationId, cancellationToken);
        if (affiliation is null)
            throw new AffiliationNotFoundException(request.AffiliationId);
        
        repository.Delete(affiliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
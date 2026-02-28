using UniMeet.Shared.Abstractions;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.AddAffiliation;

public class AddAffiliationCommandHandler(IUserAffiliationRepository repository) : ICommandHandler<AddAffiliationCommand>
{
    public async Task HandleAsync(AddAffiliationCommand request, CancellationToken cancellationToken = default)
    {
        var affiliation = new UserAffiliation(
            request.UserId,
            request.FieldOfStudyId
        );
        
        await repository.AddAsync(affiliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
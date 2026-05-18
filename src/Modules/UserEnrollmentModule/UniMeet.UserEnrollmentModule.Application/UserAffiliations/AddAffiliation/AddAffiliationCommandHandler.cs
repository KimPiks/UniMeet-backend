using UniMeet.Shared.Abstractions;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.AddAffiliation;

public class AddAffiliationCommandHandler(IUserAffiliationRepository repository) : ICommandHandler<AddAffiliationCommand>
{
    public async Task HandleAsync(AddAffiliationCommand request, CancellationToken cancellationToken = default)
    {
        var affiliation = await repository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (affiliation is null)
        {
            affiliation = new UserAffiliation(request.UserId, request.FieldOfStudyId);
            await repository.AddAsync(affiliation, cancellationToken);
        }
        else
        {
            affiliation.UpdateFieldOfStudy(request.FieldOfStudyId);
        }

        await repository.SaveChangesAsync(cancellationToken);
    }
}
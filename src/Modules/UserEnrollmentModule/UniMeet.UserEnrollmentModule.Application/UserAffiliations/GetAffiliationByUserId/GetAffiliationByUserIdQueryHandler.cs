using UniMeet.Shared.Abstractions;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.GetAffiliationByUserId;

public class GetAffiliationByUserIdQueryHandler(IUserAffiliationRepository repository) : IQueryHandler<GetAffiliationByUserIdQuery, UserAffiliationDto?>
{
    public async Task<UserAffiliationDto?> HandleAsync(GetAffiliationByUserIdQuery request, CancellationToken cancellationToken = default)
    {
        var affiliation = await repository.GetByUserIdAsync(request.UserId, cancellationToken);
        return affiliation is null
            ? null
            : new UserAffiliationDto(affiliation.Id, affiliation.UserId, affiliation.FieldOfStudyId);
    }
}

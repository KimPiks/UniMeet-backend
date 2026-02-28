using UniMeet.Shared.Abstractions;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.GetAffiliationByUserId;

public class GetAffiliationByUserIdQueryHandler(IUserAffiliationRepository repository) : IQueryHandler<GetAffiliationByUserIdQuery, UserAffiliation?>
{
    public async Task<UserAffiliation?> HandleAsync(GetAffiliationByUserIdQuery request, CancellationToken cancellationToken = default)
    {
        return await repository.GetByUserIdAsync(request.UserId, cancellationToken);
    }
}
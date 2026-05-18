using ModularSystem.Contracts.UserEnrollment;
using UniMeet.Shared.Abstractions;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.GetFieldOfStudyIdsByUserIds;

public class GetFieldOfStudyIdsByUserIdsQueryHandler(IUserAffiliationRepository repository)
    : IQueryHandler<GetFieldOfStudyIdsByUserIdsQuery, IDictionary<Guid, int>>
{
    public async Task<IDictionary<Guid, int>> HandleAsync(
        GetFieldOfStudyIdsByUserIdsQuery request,
        CancellationToken cancellationToken = default)
    {
        return await repository.GetFieldOfStudyIdsByUserIdsAsync(request.UserIds, cancellationToken);
    }
}

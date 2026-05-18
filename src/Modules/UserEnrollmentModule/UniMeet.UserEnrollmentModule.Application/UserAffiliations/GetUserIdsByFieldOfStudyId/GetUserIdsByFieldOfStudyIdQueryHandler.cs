using ModularSystem.Contracts.UserEnrollment;
using UniMeet.Shared.Abstractions;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.GetUserIdsByFieldOfStudyId;

public class GetUserIdsByFieldOfStudyIdQueryHandler(IUserAffiliationRepository repository)
    : IQueryHandler<GetUserIdsByFieldOfStudyIdQuery, IReadOnlyList<Guid>>
{
    public async Task<IReadOnlyList<Guid>> HandleAsync(
        GetUserIdsByFieldOfStudyIdQuery request,
        CancellationToken cancellationToken = default)
    {
        return await repository.GetUserIdsByFieldOfStudyIdAsync(request.FieldOfStudyId, cancellationToken);
    }
}

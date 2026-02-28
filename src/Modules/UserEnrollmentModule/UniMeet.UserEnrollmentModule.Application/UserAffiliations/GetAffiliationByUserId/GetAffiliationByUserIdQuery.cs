using UniMeet.Shared.Abstractions;
using UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

namespace UniMeet.UserEnrollmentModule.Application.UserAffiliations.GetAffiliationByUserId;

public record GetAffiliationByUserIdQuery(Guid UserId) : IQuery<UserAffiliation?>;
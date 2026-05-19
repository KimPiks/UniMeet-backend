using UniMeet.Shared.Abstractions;
using ModularSystem.Contracts.UserEnrollment.UserAffiliations;


namespace ModularSystem.Contracts.UserEnrollment.UserAffiliations.GetAffiliationByUserId;

public record GetAffiliationByUserIdQuery(Guid UserId) : IQuery<UserAffiliationDto?>;

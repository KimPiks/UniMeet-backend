using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;

public record GetAllowedEmailDomainsByUniversityIdQuery(int UniversityId, int Offset, int Limit) : IQuery<IEnumerable<AllowedEmailDomainDto>>;
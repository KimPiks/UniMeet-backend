using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;

public record GetAllowedEmailDomainsByUniversityIdQuery(int UniversityId) : IRequest<IEnumerable<AllowedEmailDomainDto>>;
using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Application.DTOs;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainsByUniversityId;

public record GetAllowedEmailDomainsByUniversityIdQuery(int UniversityId) : IRequest<IEnumerable<AllowedEmailDomainDto>>;
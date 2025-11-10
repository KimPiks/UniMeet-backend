using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.AllowedEmailDomains.GetAllowedEmailDomainById;

public record GetAllowedEmailDomainByIdQuery(int UniversityId, int DomainId) : IRequest<AllowedEmailDomainDto?>;